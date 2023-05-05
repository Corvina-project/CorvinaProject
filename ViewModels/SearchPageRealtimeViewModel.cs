using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using System.Net.Http.Json;
using Device = MauiAuth0App.Models.Device;
using Encoding = System.Text.Encoding;

namespace MauiAuth0App.ViewModels
{
    public partial class SearchPageRealtimeViewModel : ObservableObject
    {
        //https://app.corvina.io/svc/platform/api/v1/organizations/{organizationId}/devices/{deviceId}/tags?modelPath=%2A%2A&since=2000-03-11T17%3A35%3A44.652Z&to=2050-01-01T00%3A00%3A00.000Z&sinceAfter=false&limit=1000&format=json&timestampFormat=unix&aggregation=%7B%22type%22%3A%22average%22%2C%22sampling%22%3A%7B%22extent%22%3A120%2C%22size%22%3A2%2C%22unit%22%3A%22minutes%22%7D%7D
        private Token _token;
        private Device _device;
        private bool _execute;
        private int _organizationId;
        [ObservableProperty]
        public List<Tag> Tags;
        static HttpClient client = new();

        public SearchPageRealtimeViewModel(Token tk, Device dv, int id)
        {
            _token = tk;
            _device = dv;
            _execute = true;
            _organizationId = id;
            Tags = new();
            GetAllTags();
        }

        private async void GetAllTags()
        {
            //TODO Tags =; prende i tag dal json
        }

        [RelayCommand]
        private async void SelectItem(Tag tag)
        {
            string action = null;
            if (_execute && tag != null)
            {
                string tagName = tag.deviceId.ToString();
                action = await App.Current.MainPage.DisplayActionSheet("what do you want to do?", "Cancel", null, "add tag value", "view tag value");
                if (action != null && action == "add tag value")
                {
                    string newTagValueString = await App.Current.MainPage.DisplayPromptAsync("Enter the value", "Enter the value that the tag should take");
                    if (newTagValueString != null)
                    {
                        try
                        {
                            int newtagValue = int.Parse(newTagValueString);
                            if (tagName != null && newtagValue != null)
                            {
                                bool response = await AddDeviceTagValue(tagName, newtagValue, _device, _organizationId);
                                if (response)
                                {
                                    await App.Current.MainPage.DisplayAlert("Confirmation", $"You have set the tag: {tagName} to the value: {newTagValueString}", "Ok");
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Error", "the fields are invalid or you are not connected to the internet", "Ok");
                                }
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Error", "the fields are invalid or you are not connected to the internet", "Ok");
                            }
                        }

                        catch
                        {
                            await App.Current.MainPage.DisplayAlert("Error", "the fields are invalid or you are not connected to the internet", "Ok");
                        }
                    }
                }
                else
                {
                    action = await App.Current.MainPage.DisplayActionSheet("what do you want to see?", "Cancel", null, "Last Values", "Last 15 minutes", "Past day", "Past month", "Past year");
                    if (action != "Cancel")
                    {
                        _execute = false;
                        Tags = await FoundValueTagDevice(tagName, _device, _organizationId, action);
                        //DataService.general = strings;
                    }
                }
            }
        }

        private async Task<bool> AddDeviceTagValue(string tagName, int tagValue, Device device, int orgId)
        {
            try
            {
                await TokenHandler.ExecuteWithPermissionToken(client, async ()=> await client.PostAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{orgId}/devices/{device.Id}/tags", new StringContent("{\"data\": [{\"modelPath\": \"" + tagName + "\",\"v\": " + tagValue + "}] }", Encoding.UTF8, "application/json")));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<List<Tag>> FoundValueTagDevice(string tagName, Device device, int orgId, string action)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                DateTime date;
                switch (action)
                {
                    case "Last 15 minutes":
                        date = now.AddMinutes(-15);
                        break;
                    case "Past day":
                        date = now.AddDays(-1);
                        break;
                    case "Past month":
                        date = now.AddMonths(-1);
                        break;
                    case "Past year":
                        date = now.AddYears(-1);
                        break;
                    default:
                        date = now;
                        break;
                }
                var final = $"since={date:s.fffZ}&to={now:s.fffZ}";
                var url = $"https://app.corvina.io/svc/platform/api/v1/organizations/{orgId}/devices/{device.Id}/tags?modelPath={tagName}&limit={(action != "Last Values" ? 1000 : 1)}&{final}";
                Tag tags = await TokenHandler.ExecuteWithPermissionToken(client, async () => await client.GetFromJsonAsync<Tag>(url));
                
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
    }
}