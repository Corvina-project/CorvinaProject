using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using System.Text.Json;
using Device = MauiAuth0App.Models.Device;
using Encoding = System.Text.Encoding;

namespace MauiAuth0App.ViewModels
{
    public partial class TagPageViewModel : ObservableObject
    {
        private readonly Device _device;
        private bool _execute;
        private readonly string _organizationId;
        [ObservableProperty]
        public List<Tag> tags;
        private HttpClient client;
        private string text;

        public TagPageViewModel(Device dv, HttpClient client)
        {
            _device = dv;
            _execute = true;
            _organizationId = dv.OrgResourceId;
            this.client = client;
            Tags = new();
        }

        public string Text
        {
            get => text;
            set
            {
                text = value.ToLower() ?? "";
                OnPropertyChanged();
                //TODO: aggiorna lista
                //Tags = Tags.Where(t => t.modelPath.ToLower().Contains(text)).ToList();
            }
        }
        
        public async Task GetAllTags()
        {
            Tags = await FindTagsDevice(_organizationId, _device);
        }

        [RelayCommand]
        private async void SelectItem(Tag tag) {
            if (!_execute || tag == null)
                return;

            string tagName = tag.modelPath;
            string action = await Application.Current.MainPage.DisplayActionSheet("Cosa vuoi fare?", "Indietro", null, "Aggiungi un valore", "Visualizza i valori");

            if (action == null)
                return;

            if (action == "Visualizza i valori") {
                action = await Application.Current.MainPage.DisplayActionSheet("Cosa vuoi vedere?", "Annulla", null, "Ultimi valori", "Ultimi 15 minuti", "Ultimo giorno", "Ultimo mese", "Past year");
                if (action != "Cancel") {
                    _execute = false;
                    Tags = await FoundValueTagDevice(tagName, _device, _organizationId, action);
                }
                return;
            }

            if (action == "Aggiungi un valore") {
                string newTagValueString = await Application.Current.MainPage.DisplayPromptAsync("Inserisci il valore", "Inserisci il valore che il tag dovrebbe assumere");
                if (newTagValueString == null)
                    return;

                var success = int.TryParse(newTagValueString, out int newtagValue);

                if (!success) {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Il campo inserito non è valido", "Ok");
                    return;
                }

                if (tagName == null) {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Potresti non essere connesso ", "Ok");
                    return;
                }

                bool response = await AddDeviceTagValue(tagName, newtagValue, _device, _organizationId);
                if (response) {
                    await Application.Current.MainPage.DisplayAlert("Confirmation", $"You have set the tag: {tagName} to the value: {newTagValueString}", "Ok");
                } else {
                    await Application.Current.MainPage.DisplayAlert("Errore", "Connessione a internet assente", "Ok");
                }
            }
        }

        private async Task<List<Tag>> FindTagsDevice(string organizationId, Device device)
        {
            try
            {
                string json = await TokenHandler.ExecuteWithPermissionToken(client, 
                    async()=> await client.GetStringAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{device.DeviceId}/tags?modelPath=%2A%2A&since=2000-03-11T17%3A35%3A44.652Z&to=2050-01-01T00%3A00%3A00.000Z&sinceAfter=false&limit=1000&format=json&timestampFormat=unix&aggregation=%7B%22type%22%3A%22average%22%2C%22sampling%22%3A%7B%22extent%22%3A120%2C%22size%22%3A2%2C%22unit%22%3A%22minutes%22%7D%7D"));
                List<Tag> tagsList = new();
                Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
                foreach (var item in deviceTag)
                {
                    tagsList.Add(item);
                }
                return tagsList;
            }
            catch
            {
                return null;
            }
        }

        private async Task<bool> AddDeviceTagValue(string tagName, int tagValue, Device device, string organizationId)
        {
            try
            {
                await TokenHandler.ExecuteWithPermissionToken(client, 
                    async ()=> await client.PostAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{device.DeviceId}/tags", new StringContent("{\"data\": [{\"modelPath\": \"" + tagName + "\",\"v\": " + tagValue + "}] }", Encoding.UTF8, "application/json")));
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
                        tagsList.Add(new Tag() { tagValue = "Date: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nValue: " + item[1] });
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
                        //TODO: cambiare convertitore
                        tagsList.Add(new Tag() { tagValue = "Date: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nValue: " + item[1] });
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