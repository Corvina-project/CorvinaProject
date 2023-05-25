using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MauiAuth0App.Auth0;
using MauiAuth0App.Extensions;
using MauiAuth0App.Models;
using Plugin.LocalNotification;

namespace MauiAuth0App.Platforms.Android;

[Service(ForegroundServiceType = ForegroundService.TypeDataSync)]
public class BackGroundService : Service, IServices
{
    public override IBinder OnBind(Intent intent)
    {
        throw new NotImplementedException();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags,
        int startId)
    {
        if (intent.Action == "START_SERVICE")
        {
            System.Diagnostics.Debug.WriteLine("Servizio iniziato");
            RegisterNotification();
        }
        else if (intent.Action == "STOP_SERVICE")
        {
            System.Diagnostics.Debug.WriteLine("Servizio Finito");
            StopForeground(true);
            StopSelfResult(startId);
        }

        return StartCommandResult.NotSticky;
    }

    public void Start()
    {
        var startService = new Intent(MainActivity.ActivityCurrent, typeof(BackGroundService));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        var stopIntent = new Intent(MainActivity.ActivityCurrent, Class);
        stopIntent.SetAction("STOP_SERVICE");

        TokenHolder.Timer.Dispose();
        MainActivity.ActivityCurrent.StartService(stopIntent);
    }

    public void StartTokenHandler(HttpClient client)
    {
        TokenHolder.Timer = new Timer(async _ =>await RefreshAuth(client),
            null, TimeSpan.Zero,
            TimeSpan.FromSeconds(1500*1000));
        Execute(client);
    }
    
    private async Task RefreshAuth(HttpClient client) {
        List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>> {
            new("grant_type", "refresh_token"),
            new("refresh_token", TokenHolder.RefreshToken),
            new("client_id", "nextel-mobile-app")
        };
        var content = new FormUrlEncodedContent(postData);
        var response = await client.PostAsync("https://auth.corvina.io/auth/realms/exor/protocol/openid-connect/token", content);

        Token result = await JsonSerializer.DeserializeAsync<Token>(await response.Content.ReadAsStreamAsync());
        TokenHolder.AccessToken = result.AccessToken;
        TokenHolder.RefreshToken = result.RefreshToken;
        if (TokenHolder.ResourceId != null)
            await TokenHandler.GetPermissionToken(client);
        
        var request = new NotificationRequest()
        {
            NotificationId = 999,
            Title = "Alert",
            Description = "Token aggiornato: " + TokenHolder.AccessToken,
            BadgeNumber = 1
        };

        await LocalNotificationCenter.Current.Show(request);

        //await Application.Current.MainPage.DisplayAlert("Refresh", "Refresh del token fatto", "ok");
    }
    
    public static async Task<Alarms> GetAlarms(HttpClient client)
    {
        return await TokenHandler.ExecuteWithPermissionToken(client, async () =>
        {
            string richiesta = $"\"data\":\"(status != \\\"CLEARED\\\" and ( status == \\\"ACTIVE\\\" or ack == \\\"REQUIRED\\\" or reset == \\\"REQUIRED\\\" ) )\", \"orderDir\":\"asc\", \"page\":{0}, \"scopedOrganization\":\"{TokenHolder.ResourceId}\"";
            richiesta = '{' + richiesta + '}';
            var response = await client.PostAsync("https://app.corvina.io/svc/platform/api/v1/alarms/search", new StringContent(richiesta, Encoding.UTF8, "application/json"));
            return await response.Content.ReadFromJsonAsync<Alarms>();
        });
    }

    private void RegisterNotification()
    {
        var channel = new NotificationChannel("ServiceChannel", "Service", NotificationImportance.None);
        var manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(NotificationService);
        manager.CreateNotificationChannel(channel);
        var notification = new Notification.Builder(this, "ServiceChannel")
            .SetContentTitle("")
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetOngoing(true)
            .Build();

        StartForeground(100, notification);
    }
    
    private void Execute(HttpClient client)
    {
        Task.Run(async () =>
        {
            int i = 0;
            while (true)
            {
                await Task.Delay(2000);
                if (TokenHolder.ResourceId == null) continue;

                var alarms = await GetAlarms(client);

                alarms.Data.ForEach(async x =>
                {
                    i++;
                    var request = new NotificationRequest()
                    {
                        NotificationId = i,
                        Title = "Alert",
                        Description = x.Description,
                        BadgeNumber = 1
                    };
                    await LocalNotificationCenter.Current.Show(request);

                });
            }
        });
    }
    
}

