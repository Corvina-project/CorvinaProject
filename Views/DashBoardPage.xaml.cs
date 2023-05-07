using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MauiAuth0App.Auth0;
using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;

namespace MauiAuth0App.Views;

public partial class DashBoardPage : ContentPage
{
    private DashBoardViewModel viewModel;

    public DashBoardPage(HttpClient _client, Organization organization)
    {
        InitializeComponent();

        viewModel = new DashBoardViewModel(_client, organization);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        await viewModel.GetDashBoard();
        base.OnAppearing();
    }
}