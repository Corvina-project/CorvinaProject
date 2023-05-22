using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.Views;

public partial class TagDetailsPage : ContentPage
{
    private TagDetailsPageViewModel viewModel;
    public TagDetailsPage(Tag tag, Device device, HttpClient client)
    {
        InitializeComponent();
        viewModel = new TagDetailsPageViewModel(tag, device, client);
        BindingContext = viewModel;
    }
}