﻿using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Device = MauiAuth0App.Models.Device;
using Encoding = System.Text.Encoding;

namespace MauiAuth0App.ViewModels
{
    public partial class SearchPageRealtimeViewModel : ObservableObject
    {
        //TODO: binding costruttore
        private readonly Device _device;
        private bool _execute;
        private readonly int _organizationId;
        [ObservableProperty]
        public List<Tag> _Tags;
        static HttpClient client = new();

        public SearchPageRealtimeViewModel(Device dv, int id)
        {
            _device = dv;
            _execute = true;
            _organizationId = id;
            _Tags = new();
            GetAllTags();
        }

        private async void GetAllTags()
        {
            _Tags = await FindTagsDevice(_organizationId, _device);
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
                        _Tags = await FoundValueTagDevice(tagName, _device, _organizationId, action);
                        //DataService.general = strings;
                    }
                }
            }
        }

        private async Task<List<Tag>> FindTagsDevice(int orgId, Device device)
        {
            try
            {
                string json = await TokenHandler.ExecuteWithPermissionToken(client, async()=> await client.GetStringAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{orgId}/devices/{device.Id}/tags?modelPath=%2A%2A&since=2000-03-11T17%3A35%3A44.652Z&to=2050-01-01T00%3A00%3A00.000Z&sinceAfter=false&limit=1000&format=json&timestampFormat=unix&aggregation=%7B%22type%22%3A%22average%22%2C%22sampling%22%3A%7B%22extent%22%3A120%2C%22size%22%3A2%2C%22unit%22%3A%22minutes%22%7D%7D"));
                List<Tag> tags = new();
                Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
                foreach (var item in deviceTag)
                {
                    tags.Add(item);
                }
                return tags;
            }
            catch
            {
                return null;
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
                DateTime now = DateTime.UtcNow.ToUniversalTime();
                DateTime date = new();
                if (action != "Last Values")
                {
                    switch (action)
                    {
                        case "Last 15 minutes":
                            date = now.AddMinutes(-15).ToUniversalTime();
                            break;
                        case "Past day":
                            date = now.AddDays(-1).ToUniversalTime();
                            break;
                        case "Past month":
                            date = now.AddMonths(-1).ToUniversalTime();
                            break;
                        case "Past year":
                            date = now.AddYears(-1).ToUniversalTime();
                            break;
                        default:
                            date = now;
                            break;
                    }
                    string arrivo = now.ToString("s") + ".000Z";
                    string inizio = date.ToString("s") + ".000Z";
                    string final = "since=" + inizio + "&to=" + arrivo;
                    var url = $"https://app.corvina.io/svc/platform/api/v1/organizations/{orgId}/devices/{device.Id}/tags?modelPath={tagName}&limit={(action != "Last Values" ? 1000 : 1)}&{final}";
                    List<Tag> tags = await TokenHandler.ExecuteWithPermissionToken(client, async () => await client.GetFromJsonAsync<List<Tag>>(url));
                    return tags;
                }
                else
                {
                    //TODO: non stringhe, ma tag
                    /*
                    var response = await TokenHandler.ExecuteWithPermissionToken(client, async () => await client.GetAsync($"https://app.corvina.io/svc/platform/api/v1/organizations/{orgId}/devices/{device.Id}/tags?modelPath={tagName}&limit=1"));
                    List<string> tags = new();
                    string json = await response.Content.ReadAsStringAsync();
                    Tag[] deviceTag = JsonSerializer.Deserialize<Tag[]>(json);
                    foreach (var item in deviceTag[0].data)
                    {
                        tags.Add("Date: " + UnixTimeStampToDateTime(item[0].ToString()) + "\nValue: " + item[1].ToString());
                    }
                    return tags; 
                    */
                    return null;
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