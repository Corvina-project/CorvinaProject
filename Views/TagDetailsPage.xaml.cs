using LiveChartsCore.Defaults;
using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;
using System.Collections.ObjectModel;
using Device = MauiAuth0App.Models.Device;

namespace MauiAuth0App.Views;

public partial class TagDetailsPage : ContentPage
{
    private TagDetailsPageViewModel viewModel;
    public TagDetailsPage(Tag tag, Device device, HttpClient client)
    {
        TagDetailsPageViewModel.Data = new ObservableCollection<DateTimePoint>(tag.Dati.Select(pair => {
            return new DateTimePoint(pair.Key, pair.Value);
        }));
        viewModel = new TagDetailsPageViewModel(tag, device, client);
        BindingContext = viewModel;
        InitializeComponent();
    }
}