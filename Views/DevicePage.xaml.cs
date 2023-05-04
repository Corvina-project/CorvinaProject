using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiAuth0App.ViewModels;
using Device = MauiAuth0App.Models.Device;
using DeviceType = MauiAuth0App.Enum.DeviceType;

namespace MauiAuth0App.Views;

public partial class DevicePage : ContentPage
{
    public DevicePage(Device deviceModel, DeviceType deviceType)
    {
        InitializeComponent();
        BindingContext = new DeviceViewModel(deviceModel, deviceType);
    }
}