using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using MauiAuth0App.Extensions;
using MauiAuth0App.ViewModels;
using DeviceType = MauiAuth0App.Enum.DeviceType;
using MauiAuth0App.Resources.Languages;
using System.Globalization;

namespace MauiAuth0App.Views;

public partial class OrganizationsPage : ContentPage
{
    private OrganizazionsPageViewModel model;
	private readonly HttpClient client;
    private bool isBusy = false;
    public LocalizationResourceManager LocalizationResourceManager
        => LocalizationResourceManager.Instance;

    public OrganizationsPage(HttpClient client, IServices services) {
		InitializeComponent();
		this.client = client;
        model = new OrganizazionsPageViewModel(client, services);
        BindingContext = model;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await model.LoadOrganizations();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) {
        if (isBusy)
            return;
        isBusy = true;
        var organization = (sender as BindableObject).BindingContext as Organization;
        var page = new OptionsPage(client);
        var model = new OptionsPageViewModel(client, organization);
        await model.LoadViewModel();
        page.BindingContext = model;
        await Navigation.PushAsync(page);
        isBusy = false;
    }
}