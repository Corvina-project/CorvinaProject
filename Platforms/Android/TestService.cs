using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using MauiAuth0App.Extensions;
using Plugin.LocalNotification;

namespace MauiAuth0App.Platforms.Android;

[Service(ForegroundServiceType = ForegroundService.TypeDataSync)]
public class TestService : Service, IServices
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
            
            Execute();
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
        var startService = new Intent(MainActivity.ActivityCurrent, typeof(TestService));
        startService.SetAction("START_SERVICE");
        MainActivity.ActivityCurrent.StartService(startService);
    }

    public void Stop()
    {
        var stopIntent = new Intent(MainActivity.ActivityCurrent, Class);
        stopIntent.SetAction("STOP_SERVICE");
        MainActivity.ActivityCurrent.StartService(stopIntent);
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
    
    private void Execute()
    {
        Task.Run(async () =>
        {
            int i = 0;
            while (true)
            {
                var request = new NotificationRequest()
                {
                    NotificationId = i,
                    Title = "Alert",
                    Description = i.ToString(),
                    BadgeNumber = 1
                };

                await LocalNotificationCenter.Current.Show(request);
                await Task.Delay(3000);
                i++;
            }
        });
    }
    
}

