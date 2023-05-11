using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MauiAuth0App.ViewModels;
using DeviceType = MauiAuth0App.Enum.DeviceType;

namespace MauiAuth0App.Views;

public partial class OrganizationsPage : ContentPage
{

    private OrganizazionsPageViewModel model;
	private readonly HttpClient client;

	public OrganizationsPage(HttpClient client) {
		InitializeComponent();
		this.client = client;

        model = new OrganizazionsPageViewModel(client);
        BindingContext = model;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await model.LoadOrganizations();
    }

    private async void OrganizationsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TokenHolder.ResourceId = model.SelectedOrganization.ResourceId;
        
        await model.LoadDevices();
        await model.LoadAlarms();
        await model.LoadDashBoards();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) {
        //TODO: Sta parte è inutile se visualizziamo tutto nella homepage
        //string action = await DisplayActionSheet("Cosa vuoi aprire", "Cancel", null, "Device", "Alarms", "Dashboard");

        var device = (sender as BindableObject).BindingContext as Models.Device;
        await Navigation.PushAsync(new DevicePage(device, DeviceType.Device, client));

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