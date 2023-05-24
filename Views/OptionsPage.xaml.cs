using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;

namespace MauiAuth0App.Views;

public partial class OptionsPage : ContentPage
{

    private HttpClient client;

	public OptionsPage(HttpClient client)
	{
        this.client = client;
		InitializeComponent();
	}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) {
        //TODO: Sta parte è inutile se visualizziamo tutto nella homepage
        //string action = await DisplayActionSheet("Cosa vuoi aprire", "Cancel", null, "Device", "Alarms", "Dashboard");

        var device = (sender as BindableObject).BindingContext as Models.Device;
        await Navigation.PushAsync(new DevicePage(device, Enum.DeviceType.Device, client));

        /*
        switch (action)
        {
            case "Device": await Navigation.PushAsync(new DevicePage(device, DeviceType.Device, client)); break;
            case "Alarms": await Navigation.PushAsync(new DevicePage(device, DeviceType.Device, client)); break;
            case "Dashboard": await Navigation.PushAsync(new DashBoardPage(client, model.SelectedOrganization)); break;
        }
        */
    }
}