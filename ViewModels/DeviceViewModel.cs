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
    [ObservableProperty] private bool? isConnected;
    [ObservableProperty] private string isVpnConnected;
    private HttpClient client;

    public DeviceViewModel(Device deviceModel, DeviceType deviceType, HttpClient client)
    {
        this.deviceModel = deviceModel;
        this.deviceType = deviceType;
        this.client = client;
        IsConnected = deviceModel.Connected;
        switch (deviceModel.Attributes.VpnConnected)
        {
            case true:
                IsVpnConnected = Resources.Languages.Language.Connected;
                break;
            case false:
                IsVpnConnected = Resources.Languages.Language.NotConnected;
                break;
        }
    }

    [RelayCommand]
    public async Task GoToTagPage()
    {
        await App.Current.MainPage.Navigation.PushAsync(new TagPage(DeviceModel, client));
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