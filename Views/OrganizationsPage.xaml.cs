using MauiAuth0App.Models;
using System.Net.Http.Json;

namespace MauiAuth0App.Views;

public partial class OrganizationsPage : ContentPage {

	private readonly HttpClient client;

	public OrganizationsPage(HttpClient client) {
		InitializeComponent();
		this.client = client;
	}

    protected override async void OnAppearing() {
        base.OnAppearing();
		model.Organizations = await client.GetFromJsonAsync<List<Organization>>("core/api/v1/organizations/mine");
    }

}