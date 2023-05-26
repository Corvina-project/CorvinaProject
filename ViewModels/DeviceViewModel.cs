using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Views;
using Device = MauiAuth0App.Models.Device;
using DeviceType = MauiAuth0App.Enum.DeviceType;

namespace MauiAuth0App.ViewModels;

public partial class DeviceViewModel : ObservableObject
{
    [ObservableProperty] private Device deviceModel;
    [ObservableProperty] private DeviceType deviceType;
    private HttpClient client;
    IMap map;

    public DeviceViewModel(Device deviceModel, DeviceType deviceType, HttpClient client)
    {
        this.deviceModel = deviceModel;
        this.deviceType = deviceType;
        this.client = client;
    }

    [RelayCommand]
    public async Task GoToTagPage()
    {
        await App.Current.MainPage.Navigation.PushAsync(new TagPage(DeviceModel, client));
        //await App.Current.MainPage.DisplayAlert("Tag Page", "andare a tag page", "ok");
    }

    [RelayCommand]
    private async Task OpenMap()
    {
        try
        {
            var location = new Location() { Latitude = DeviceModel.Attributes.GeoLocation[0], Longitude = DeviceModel.Attributes.GeoLocation[1] };
            await location.OpenMapsAsync(new MapLaunchOptions
            {
                Name = DeviceModel.Name,
                NavigationMode = NavigationMode.None
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error, Maps app!", ex.Message, "OK");
        }
    }
}