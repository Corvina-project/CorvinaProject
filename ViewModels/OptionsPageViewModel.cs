using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using MauiAuth0App.Views;
using System.Net.Http.Json;
using MauiAuth0App.Extensions;

namespace MauiAuth0App.ViewModels {
    public partial class OptionsPageViewModel : ObservableObject {

        [ObservableProperty] private Organization organization;

        [ObservableProperty] private List<Models.Device> devices = new();
        [ObservableProperty] private List<Alarm> alarms = new();
        [ObservableProperty] private DashBoards dashBoards = new();

        private HttpClient client;
        private IServices services;

        public OptionsPageViewModel(HttpClient client, Organization organization) {
            this.client = client;
            Organization = organization;
        }

        public async Task LoadViewModel(IServices services) {
            TokenHolder.ResourceId = Organization.ResourceId;

            this.services = services;

            await LoadDevices();
            await LoadAlarms();
            await LoadDashBoards();
        }

        public async Task LoadDevices() {
            var device = await TokenHandler.ExecuteWithPermissionToken(client,
                () => client.GetFromJsonAsync<Devices>($"mappings/api/v1/devices?page=0&pageSize=25&orderBy=&orderDir=&append=false&search=&organization={TokenHolder.ResourceId}")
            );

            Devices = device.Data;
        }

        public async Task LoadAlarms() {
            string richiesta = $"\"data\":\"(status != \\\"CLEARED\\\" and ( status == \\\"ACTIVE\\\" or ack == \\\"REQUIRED\\\" or reset == \\\"REQUIRED\\\" ) )\", \"orderDir\":\"asc\", \"page\":0, \"scopedOrganization\":\"{TokenHolder.ResourceId}\"";
            richiesta = '{' + richiesta + '}';

            var alarm = await TokenHandler.ExecuteWithPermissionToken(client,
                async () => {
                    var result = await client.PostAsync("https://app.corvina.io/svc/platform/api/v1/alarms/search", new StringContent(richiesta, System.Text.Encoding.UTF8, "application/json"));
                    return await result.Content.ReadFromJsonAsync<Alarms>();
                });

            Alarms = alarm.Data;
        }

        public async Task LoadDashBoards() {
            DashBoards = await TokenHandler.ExecuteWithPermissionToken(
                client,
                () => client.GetFromJsonAsync<DashBoards>(
                    $"https://app.corvina.io/svc/mappings/api/v1/dashboards/mine?page=0&search=&append=true&orderBy=&orderDir=&organization={Organization.ResourceId}&pageSize=100")
            );
        }

        [RelayCommand]
        private async Task OpenDashBoard(DashBoard data) {
            //await App.Current.MainPage.Navigation.PushAsync(
            //    new WebViewPage($"https://app.corvina.io/#/dashboards/{data.Id}/new?org={data.OrgResourceId}&org={data.OrgResourceId}"));
            await App.Current.MainPage.Navigation.PushAsync(
                new WebViewPage($"https://app.corvina.io/#/dashboards/{data.Id}"));
        }

        [RelayCommand]
        private async Task Logout()
        {
            services.Stop();
            TokenHolder.ClearToken();
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}
