using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.ViewModels
{
    public class TagDetailsPageViewModel
    {
        private readonly Device _device;
        private readonly string _organizationId;
        private HttpClient client;
        public TagDetailsPageViewModel(HttpClient client, Tag tg, Device dv)
        {
            this.client = client;
            _device = dv;
            _organizationId = dv.OrgResourceId;
        }

        //[RelayCommand]
        //private async void SelectItem(Tag tag) {
        //    if (!_execute || tag == null)
        //        return;

        //    string tagName = tag.modelPath;
        //    string action = await Application.Current.MainPage.DisplayActionSheet("Cosa vuoi fare?", "Indietro", null, "Aggiungi un valore", "Visualizza i valori");

        //    if (action == null)
        //        return;

        //    if (action == "Visualizza i valori") {
        //        action = await Application.Current.MainPage.DisplayActionSheet("Cosa vuoi vedere?", "Annulla", null, "Ultimi valori", "Ultimi 15 minuti", "Ultimo giorno", "Ultimo mese", "Ultimo anno");
        //        if (action != "Annulla") {
        //            _execute = false;
        //            Tags = await FoundValueTagDevice(tagName, _device, _organizationId, action);
        //        }
        //        return;
        //    }

        //    if (action == "Aggiungi un valore") {
        //        string newTagValueString = await Application.Current.MainPage.DisplayPromptAsync("Inserisci il valore", "Inserisci il valore che il tag dovrebbe assumere");
        //        if (newTagValueString == null)
        //            return;

        //        var success = int.TryParse(newTagValueString, out int newtagValue);

        //        if (!success) {
        //            await Application.Current.MainPage.DisplayAlert("Errore", "Il campo inserito non è valido", "Ok");
        //            return;
        //        }

        //        if (tagName == null) {
        //            await Application.Current.MainPage.DisplayAlert("Errore", "Potresti non essere connesso ", "Ok");
        //            return;
        //        }

        //        bool response = await AddDeviceTagValue(tagName, newtagValue, _device, _organizationId);
        //        if (response) {
        //            await Application.Current.MainPage.DisplayAlert("Conferma", $"Hai assegnato al tag: {tagName} il valore: {newTagValueString}", "Ok");
        //        } else {
        //            await Application.Current.MainPage.DisplayAlert("Errore", "Connessione a internet assente", "Ok");
        //        }
        //    }
        //}

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

        private async Task<List<Tag>> FoundValueTagDevice(string tagName, Device device, string organizationId, string action)
        {
            try
            {
                List<Tag> tagsList = new();
                DateTime now = DateTime.Now.ToUniversalTime();
                DateTime date = new();
                if (action != "Last Values")
                {
                    switch (action)
                    {
                        case "Ultimi 15 minuti":
                            date = now.AddMinutes(-15).ToUniversalTime();
                            break;
                        case "Ultimo giorno":
                            date = now.AddDays(-1).ToUniversalTime();
                            break;
                        case "Ultimo mese":
                            date = now.AddMonths(-1).ToUniversalTime();
                            break;
                        case "Ultimo anno":
                            date = now.AddYears(-1).ToUniversalTime();
                            break;
                        default:
                            date = now;
                            break;
                    }
                    string arrivo = now.ToString("s") + ".000Z";
                    string inizio = date.ToString("s") + ".000Z";
                    string final = "since=" + inizio + "&to=" + arrivo;
                    var response = await TokenHandler.ExecuteWithPermissionToken(client,
                        async () => await client.GetAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{device.DeviceId}/tags?modelPath={tagName}&limit=1000&{final}"));
                    string json = await response.Content.ReadAsStringAsync();
                    Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
                    foreach (var item in deviceTag[0].data)
                    {
                        tagsList.Add(new Tag() { tagValue = "Data: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nValore: " + item[1] });
                    }
                    return tagsList;
                }
                else
                {
                    var response = await TokenHandler.ExecuteWithPermissionToken(client, async () => await client.GetAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{device.DeviceId}/tags?modelPath={tagName}&limit=1"));
                    string json = await response.Content.ReadAsStringAsync();
                    Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
                    foreach (var item in deviceTag[0].data)
                    {
                        tagsList.Add(new Tag() { tagValue = "Data: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nValore: " + item[1] });
                    }
                    return tagsList;
                }
            }
            catch (Exception ex)
            {
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
