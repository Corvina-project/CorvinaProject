using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using MauiAuth0App.Views;
using MauiAuth0App.Converter;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Device = MauiAuth0App.Models.Device;
using Encoding = System.Text.Encoding;

namespace MauiAuth0App.ViewModels {
    public partial class TagPageViewModel : ObservableObject {
        private readonly Device _device;
        private readonly string _organizationId;
        private readonly object _lock = new();
        private CancellationTokenSource source = new();
        private HttpClient client;

        [ObservableProperty]
        private ObservableCollection<Tag> tags = new();
        [ObservableProperty]
        private List<Tag> searchTags = new();
        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private string searchText = "";

        public TagPageViewModel(Device device, HttpClient client) {
            this.client = client;
            this._device = device;
            _organizationId = device.OrgResourceId;
        }

        public async Task GetAllTags() {
            IsLoading = true;
            var tags = await FindTagsDevice();
            Tags = new ObservableCollection<Tag>(tags);
            await Search(SearchText);
            IsLoading = false;
        }

        public async Task Search(string text) {
            source.Cancel();
            source = new();

            await Task.Run(() => {
                var results = Tags.Where(t => t.modelPath.ToLower().Contains(text.ToLower())).ToList();
                if (source.IsCancellationRequested)
                    return;
                lock (_lock) { 
                    SearchTags = results;
                }
            }, source.Token);
        }

        [RelayCommand]
        private async void GoToTagDetailsPage(Tag tag)
        {
            await App.Current.MainPage.Navigation.PushAsync(new TagDetailsPage(tag, _device, client));
        }

        private async Task<List<Tag>> FindTagsDevice() {
            var tags = await TokenHandler.ExecuteWithPermissionToken(client,                                                                                            // modelPath=**&since=2000-03-11T17:35:44.652Z&to=2050-01-01T00:00:00.000Z&sinceAfter=false&limit=1000&format=json&timestampFormat=unix&aggregation={"type":"average","sampling":{"extent":120,"size"=3,"unit"="minutes"}}
                    () => client.GetFromJsonAsync<List<Tag>>($"https://app.corvina.io/svc/platform/api/v1/organizations/{_organizationId}/devices/{_device.DeviceId}/tags?modelPath=%2A%2A&since=2000-03-11T17%3A35%3A44.652Z&to=2050-01-01T00%3A00%3A00.000Z&sinceAfter=false&limit=1000&format=json&timestampFormat=unix")); // &limit=1000&&aggregation=%7B%22type%22%3A%22average%22%2C%22sampling%22%3A%7B%22extent%22%3A120%2C%22size%22%3A2%2C%22unit%22%3A%22minutes%22%7D%7D

            tags.ForEach(async tag => {
                foreach (var item in tag.data) {
                    var dateTime = (DateTime) UnixTimeStamp.UnixTimeStampToDateTime(item[0].ToString());
                    _ = double.TryParse(item[1].ToString(), out double valore);
                    //Application.Current.MainPage.DisplayAlert("Valore", $"{item[0]} {item[1]}", "ok");

                    tag.Dati.Add(dateTime, valore);

                    // da togliere
                    tag.tagValue = "Data: " + UnixTimeStamp.UnixTimeStampToString(item[0].ToString()) + "\nValore: " + item[1];
                }
            });
            return tags;

            #region Vecchio metodo
            /*
            try
            {
                tagsList = new();
                int index = 0;
                

                await Application.Current.MainPage.DisplayAlert("a", json, "ok");

                int k = i + 10;
                if (_executeTag && deviceTag.Length < k)
                {
                    for (int j = i; j < deviceTag.Length; j++)
                    {
                        tagsList.Add(deviceTag[j]);
                        string tagName = deviceTag[j].modelPath;
                        var response = await TokenHandler.ExecuteWithPermissionToken(client, async () =>
                            await client.GetAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{_organizationId}/devices/{_device.DeviceId}/tags?modelPath={tagName}&limit=1"));
                        string jsonTagValues = await response.Content.ReadAsStringAsync();
                        Tag[] deviceTagValues = JsonSerializer.Deserialize<Tag[]>(jsonTagValues);
                        if (deviceTagValues[0].data is null)
                            continue;
                        foreach (var item in deviceTagValues[0].data)
                        {
                            var data = (DateTime) UnixTimestampToDateTime(item[0].ToString());
                            Application.Current.MainPage.DisplayAlert("Valore", $"{item[0]} {item[1]}", "ok");

                            if (data.Ticks == 0)
                                continue;

                            tagsList[index].Data = data;
                            _ = double.TryParse(item[1].ToString(), out double valore);
                            tagsList[index].Valore = valore;

                            // da togliere
                            tagsList[index].tagValue = "Data: " + UnixTimestampToDateTimeString(item[0].ToString()) + "\nValore: " + item[1];
                        }
                        index++;
                    }
                    _executeTag = false;
                    return tagsList;
                }
                else if (!_executeTag) { return tagsList; }
                else
                {
                    for (; i < k; i++)
                    {
                        tagsList.Add(deviceTag[i]);
                        string tagName = deviceTag[i].modelPath;
                        var response = await TokenHandler.ExecuteWithPermissionToken(client, async () =>
                            await client.GetAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{_organizationId}/devices/{_device.DeviceId}/tags?modelPath={tagName}&limit=1"));
                        string jsonTagValues = await response.Content.ReadAsStringAsync();
                        Tag[] deviceTagValues = JsonSerializer.Deserialize<Tag[]>(jsonTagValues);
                        if (deviceTagValues[0].data is null)
                            continue;
                        foreach (var item in deviceTagValues[0].data)
                        {
                            var data = (DateTime) UnixTimestampToDateTime(item[0].ToString());
                            Application.Current.MainPage.DisplayAlert("Valore", $"{item[0]} {item[1]}", "ok");

                            if (data.Ticks == 0)
                                continue;

                            tagsList[index].Data = data;
                            _ = double.TryParse(item[1].ToString(), out double valore);
                            tagsList[index].Valore = valore;

                            // da togliere
                            tagsList[index].tagValue = "Data: " + UnixTimestampToDateTimeString(item[0].ToString()) + "\nValore: " + item[1];
                        }
                        index++;
                    }
                    return tagsList;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new List<Tag>();
            }
            */
            #endregion
        }
    }
}