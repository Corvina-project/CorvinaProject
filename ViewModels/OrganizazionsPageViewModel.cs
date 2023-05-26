using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiAuth0App.Models;

namespace MauiAuth0App.ViewModels {
    public partial class OrganizazionsPageViewModel : ObservableObject {

        [ObservableProperty] private List<Organization> organizations;

        private HttpClient client;

        public OrganizazionsPageViewModel(HttpClient client)
        {
            this.client = client;
        }
        
        public async Task LoadOrganizations()
        {
            Organizations = await client.GetFromJsonAsync<List<Organization>>("core/api/v1/organizations/mine");
        }

    }
}
