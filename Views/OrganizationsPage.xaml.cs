using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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

    private async void OrganizationsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        TokenHolder.ResourceId = model.SelectedOrganization.ResourceId;
        var devices = await TokenHandler.ExecuteWithPermissionToken(client,
            () => client.GetFromJsonAsync<Devices>($"mappings/api/v1/devices?page=0&pageSize=25&orderBy=&orderDir=&append=false&search=&organization={TokenHolder.ResourceId}")
        );

        model.Devices = devices.Data;

        string richiesta = $"\"data\":\"(status != \\\"CLEARED\\\" and ( status == \\\"ACTIVE\\\" or ack == \\\"REQUIRED\\\" or reset == \\\"REQUIRED\\\" ) )\", \"orderDir\":\"asc\", \"page\":0, \"scopedOrganization\":\"{TokenHolder.ResourceId}\"";
        richiesta = '{' + richiesta + '}';

        var alarms = await TokenHandler.ExecuteWithPermissionToken(client,
            async () => {
                var result = await client.PostAsync("https://app.corvina.io/svc/platform/api/v1/alarms/search", new StringContent(richiesta, System.Text.Encoding.UTF8, "application/json"));
                return await result.Content.ReadFromJsonAsync<Alarms>();
            });

        model.Alarms = alarms.Data;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) {
        await DisplayAlert("paagina", "details page", "ok");
    }
}