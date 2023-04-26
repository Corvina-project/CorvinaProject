using CommunityToolkit.Mvvm.ComponentModel;
using MauiAuth0App.Models;

namespace MauiAuth0App.ViewModels {
    public partial class OrganizazionsPageViewModel : ObservableObject {

        [ObservableProperty]
        private List<Organization> organizations;

    }
}
