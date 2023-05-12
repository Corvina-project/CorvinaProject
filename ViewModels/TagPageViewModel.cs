using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Extensions;
using MauiAuth0App.Models;
using MauiAuth0App.Views;
using System.Collections.ObjectModel;
using System.Text.Json;
using Device = MauiAuth0App.Models.Device;
using Encoding = System.Text.Encoding;

namespace MauiAuth0App.ViewModels
{
    public partial class TagPageViewModel : ObservableObject
    {
        private readonly Device _device;
        private readonly string _organizationId;
        public ObservableCollection<Tag> Tags { get; set; }
        private HttpClient client;
        private string text;
        private int i = 0;
        private List<Tag> tagsList = new();
        [ObservableProperty] private bool isLoading;
        private bool _executeTag;

        public TagPageViewModel(Device dv, HttpClient client)
        {
            _device = dv;
            _organizationId = dv.OrgResourceId;
            this.client = client;
            Tags = new();
            _executeTag = true;
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

        [RelayCommand]
        public async Task GetAllTags()
        {
            IsLoading = true;
            var tagList = await FindTagsDevice();
            foreach (var item in tagList)
            {
                Tags.Add(item);
            }
            IsLoading = false;
        }

        [RelayCommand]
        private async void GoToTagDetailsPage(Tag tag)
        {
            await App.Current.MainPage.Navigation.PushAsync(new TagDetailsPage());
        }

        private async Task<List<Tag>> FindTagsDevice()
        {
            try
            {
                tagsList = new();
                int index = 0;
                string json = await TokenHandler.ExecuteWithPermissionToken(client,
                    async () => await client.GetStringAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{_organizationId}/devices/{_device.DeviceId}/tags?modelPath=%2A%2A&since=2000-03-11T17%3A35%3A44.652Z&to=2050-01-01T00%3A00%3A00.000Z&sinceAfter=false&limit=1000&format=json&timestampFormat=unix&aggregation=%7B%22type%22%3A%22average%22%2C%22sampling%22%3A%7B%22extent%22%3A120%2C%22size%22%3A2%2C%22unit%22%3A%22minutes%22%7D%7D"));
                Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
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
                            tagsList[index].tagValue = "Data: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nUltimo valore: " + item[1];
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
                            tagsList[index].tagValue = "Data: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nUltimo valore: " + item[1];
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