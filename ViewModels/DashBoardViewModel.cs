using System.Collections.ObjectModel;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using MauiAuth0App.Views;

namespace MauiAuth0App.ViewModels;

public partial class DashBoardViewModel : ObservableObject
{
    private HttpClient client;
    private Organization organization;

    [ObservableProperty] public DashBoards dashBoards;

    public DashBoardViewModel(HttpClient client, Organization organization)
    {
        this.client = client;
        this.organization = organization;
    }

    public async Task GetDashBoard()
    {
        DashBoards = await TokenHandler.ExecuteWithPermissionToken(
            client,
            () => client.GetFromJsonAsync<DashBoards>(
                $"https://app.corvina.io/svc/mappings/api/v1/dashboards/mine?page=0&search=&append=true&orderBy=&orderDir=&organization={organization.ResourceId}&pageSize=100")
        );
    }

    [RelayCommand]
    private async Task OpenDashBoard(DashBoard data)
    {
        await App.Current.MainPage.Navigation.PushAsync(
            new WebViewPage($"https://app.corvina.io/#/dashboards/{data.Id}/new?org={data.OrgResourceId}&org={data.OrgResourceId}"));
    }
}