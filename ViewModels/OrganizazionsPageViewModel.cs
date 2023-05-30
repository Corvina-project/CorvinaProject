using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Extensions;
using MauiAuth0App.Models;
using MauiAuth0App.Views;

namespace MauiAuth0App.ViewModels {
    public partial class OrganizazionsPageViewModel : ObservableObject {

        [ObservableProperty] private List<Organization> organizations;

        private HttpClient client;
        private IServices services;

        public OrganizazionsPageViewModel(HttpClient client, IServices services)
        {
            this.services = services;
            this.client = client;
        }
        
        public async Task LoadOrganizations()
        {
            Organizations = await client.GetFromJsonAsync<List<Organization>>("core/api/v1/organizations/mine");
        }

        [RelayCommand]
        private async void GoToCredits()
        {
            await App.Current.MainPage.Navigation.PushAsync(new Settings());
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
