using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.Views;

public partial class TagPage : ContentPage
{
    private TagPageViewModel viewModel;
    public TagPage(Device device, HttpClient client)
    {
        InitializeComponent();
        viewModel = new TagPageViewModel(device, client);
        BindingContext = viewModel;
    }
}