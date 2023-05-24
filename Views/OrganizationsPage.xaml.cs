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

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) {
        var organization = (sender as BindableObject).BindingContext as Organization;
        var page = new OptionsPage(client);
        var model = new OptionsPageViewModel(client, organization);
        await model.LoadViewModel();
        page.BindingContext = model;
        await Navigation.PushAsync(page);
    }
}