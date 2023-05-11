using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using MauiAuth0App.Views;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.ViewModels {
    public partial class OrganizazionsPageViewModel : ObservableObject {

        [ObservableProperty] private List<Organization> organizations;
        [ObservableProperty] private Organization selectedOrganization;

        [ObservableProperty] private List<Device> devices = new();
        [ObservableProperty] private List<Alarm> alarms = new();
        [ObservableProperty] private DashBoards dashBoards = new();

        private HttpClient client;

        public OrganizazionsPageViewModel(HttpClient client)
        {
            this.client = client;
        }
        
        public async Task LoadOrganizations()
        {
            Organizations = await client.GetFromJsonAsync<List<Organization>>("core/api/v1/organizations/mine");
        }

        public async Task LoadDevices()
        {
            var device = await TokenHandler.ExecuteWithPermissionToken(client,
                () => client.GetFromJsonAsync<Devices>($"mappings/api/v1/devices?page=0&pageSize=25&orderBy=&orderDir=&append=false&search=&organization={TokenHolder.ResourceId}")
            );

            Devices = device.Data;
        }
        
        public async Task LoadAlarms()
        {
            string richiesta = $"\"data\":\"(status != \\\"CLEARED\\\" and ( status == \\\"ACTIVE\\\" or ack == \\\"REQUIRED\\\" or reset == \\\"REQUIRED\\\" ) )\", \"orderDir\":\"asc\", \"page\":0, \"scopedOrganization\":\"{TokenHolder.ResourceId}\"";
            richiesta = '{' + richiesta + '}';

            var alarm = await TokenHandler.ExecuteWithPermissionToken(client,
                async () => {
                    var result = await client.PostAsync("https://app.corvina.io/svc/platform/api/v1/alarms/search", new StringContent(richiesta, System.Text.Encoding.UTF8, "application/json"));
                    return await result.Content.ReadFromJsonAsync<Alarms>(); 
                });

            Alarms = alarm.Data;
        }
        
        public async Task LoadDashBoards()
        {
            DashBoards = await TokenHandler.ExecuteWithPermissionToken(
                client,
                () => client.GetFromJsonAsync<DashBoards>(
                    $"https://app.corvina.io/svc/mappings/api/v1/dashboards/mine?page=0&search=&append=true&orderBy=&orderDir=&organization={SelectedOrganization.ResourceId}&pageSize=100")
            );
        }
        
        [RelayCommand]
        private async Task OpenDashBoard(DashBoard data)
        {
            //await App.Current.MainPage.Navigation.PushAsync(
            //    new WebViewPage($"https://app.corvina.io/#/dashboards/{data.Id}/new?org={data.OrgResourceId}&org={data.OrgResourceId}"));
            await App.Current.MainPage.Navigation.PushAsync(
                new WebViewPage($"https://app.corvina.io/#/dashboards/{data.Id}"));
        }
    }
}
