using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.Views;

public partial class SearchPageRealtime : ContentPage
{
    private SearchPageRealtimeViewModel viewModel;
    public SearchPageRealtime(Device device, HttpClient client)
    {
        InitializeComponent();
        viewModel = new SearchPageRealtimeViewModel(device, client);
        BindingContext = viewModel;
    }
}