﻿using CommunityToolkit.Mvvm.ComponentModel;
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

    public DeviceViewModel(Device deviceModel, DeviceType deviceType)
    {
        this.deviceModel = deviceModel;
        this.deviceType = deviceType;
    }

    [RelayCommand]
    public async Task GoToTagPage()
    {
        await App.Current.MainPage.DisplayAlert("Tag Page", "andare a tag page", "ok");
    }
}