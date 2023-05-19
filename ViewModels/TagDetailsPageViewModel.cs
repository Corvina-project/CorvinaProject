using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.ViewModels
{
    public partial class TagDetailsPageViewModel : ObservableObject
    {
        private readonly Device _device;
        private readonly string _organizationId;
        private HttpClient client;
        [ObservableProperty] private Tag tagItem;
        [ObservableProperty] private bool isLoading;

        public TagDetailsPageViewModel(Tag tg, Device dv, HttpClient client)
        {
            this.client = client;
            _device = dv;
            _organizationId = dv.OrgResourceId;
            tagItem = tg;
        }

        [RelayCommand]
        public async Task ViewValue()
        {
            IsLoading = true;
            await SelectItem();
            IsLoading = false;
        }
        
        private async Task SelectItem()
        {
            string tagName = TagItem.modelPath;
            string action = await Application.Current.MainPage.DisplayActionSheet("Cosa vuoi fare?", "Indietro", null, "Aggiungi un valore", "Visualizza i valori");
            if (action == null)
                return;
            if (action == "Visualizza i valori")
            {
                action = await Application.Current.MainPage.DisplayActionSheet("Cosa vuoi vedere?", "Annulla", null, "Ultimi 15 minuti", "Ultimo giorno", "Ultimo mese", "Ultimo anno");
                if (action != "Annulla")
                {
                    TagItem = await FoundValueTagDevice(tagName, _device, _organizationId, action);
                }
                return;
            }
            if (action == "Aggiungi un valore")
            {
                string newTagValueString = await Application.Current.MainPage.DisplayPromptAsync("Inserisci il valore", "Inserisci il valore che il tag dovrebbe assumere");
                if (newTagValueString == null)
                    return;
                var success = int.TryParse(newTagValueString, out int newtagValue);
                if (!success)
                {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Il campo inserito non è valido", "Ok");
                    return;
                }
                if (tagName == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Potresti non essere connesso ", "Ok");
                    return;
                }
                bool response = await AddDeviceTagValue(tagName, newtagValue, _device, _organizationId);
                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Conferma", $"Hai assegnato al tag: {tagName} il valore: {newTagValueString}", "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Connessione a internet assente", "Ok");
                }
            }
        }

        private async Task<bool> AddDeviceTagValue(string tagName, int tagValue, Device device, string organizationId)
        {
            try
            {
                await TokenHandler.ExecuteWithPermissionToken(client,
                    async () => await client.PostAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{device.DeviceId}/tags", new StringContent("{\"data\": [{\"modelPath\": \"" + tagName + "\",\"v\": " + tagValue + "}] }", Encoding.UTF8, "application/json")));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<Tag> FoundValueTagDevice(string tagName, Device device, string organizationId, string action)
        {
            try
            {
                Tag tag = new();
                DateTime now = DateTime.Now.ToUniversalTime();
                DateTime date = new();
                date = action switch
                {
                    "Ultimi 15 minuti" => now.AddMinutes(-15).ToUniversalTime(),
                    "Ultimo giorno" => now.AddDays(-1).ToUniversalTime(),
                    "Ultimo mese" => now.AddMonths(-1).ToUniversalTime(),
                    "Ultimo anno" => now.AddYears(-1).ToUniversalTime(),
                    _ => now,
                };
                string arrivo = now.ToString("s") + ".000Z";
                string inizio = date.ToString("s") + ".000Z";
                string final = "since=" + inizio + "&to=" + arrivo;
                var response = await TokenHandler.ExecuteWithPermissionToken(client,
                    async () => await client.GetAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{device.DeviceId}/tags?modelPath={tagName}&limit=1000&{final}"));
                string json = await response.Content.ReadAsStringAsync();
                Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
                if (deviceTag[0].data.Length == 0)
                    tag = new Tag() { modelPath = tagName, tagValue = "Valore: valore non dato" };
                else
                {
                    foreach (var item in deviceTag[0].data)
                    {
                        tag = new Tag() { tagValue = "Data: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nValore: " + item[1], modelPath = tagName };
                    }
                }
                return tag;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }

        private static string UnixTimeStampToDateTime(string unixTime)
        {
            string response = unixTime;
            try
            {
                double c = double.Parse(unixTime);
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddMilliseconds(c).ToLocalTime();
                response = dateTime.ToString();
            }
            catch
            {
            }
            return response;
        }
    }
}
