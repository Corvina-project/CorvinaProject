using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Models;
using MauiAuth0App.Views;

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

        [RelayCommand]
        private async void GoToCredits()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Credits());
        }
    }
}
