using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using Device = MauiAuth0App.Models.Device;
using DeviceType = MauiAuth0App.Enum.DeviceType;

namespace MauiAuth0App.ViewModels;

public partial class DeviceViewModel : ObservableObject
{
    [ObservableProperty] private Device deviceModel;
    [ObservableProperty] private DeviceType deviceType;
    [ObservableProperty] private DateTime creationDate;
    [ObservableProperty] private DateTime lastUpdate;

    public DeviceViewModel(Device deviceModel, DeviceType deviceType)
    {
        this.deviceModel = deviceModel;
        this.deviceType = deviceType;
        creationDate = UnixToDateTime(deviceModel.CreationDate);
        lastUpdate = UnixToDateTime(deviceModel.LastConfigUpdateAt);
    }
    public DateTime UnixToDateTime(long unix)
    {
        DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
        DateTime dataC = date.AddMilliseconds((double)unix);
        return dataC;
    }

    [RelayCommand]
    public async Task GoToTagPage()
    {
        await App.Current.MainPage.DisplayAlert("Tag Page", "andare a tag page", "ok");
    }
}