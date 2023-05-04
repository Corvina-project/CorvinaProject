using CommunityToolkit.Mvvm.ComponentModel;
using MauiAuth0App.Models;

namespace MauiAuth0App.ViewModels {
    public partial class OrganizazionsPageViewModel : ObservableObject {

        [ObservableProperty]
        private List<Organization> organizations;

        [ObservableProperty]
        private Organization selectedOrganization;

        [ObservableProperty]
        private List<Models.Device> devices = new();

        [ObservableProperty]
        private List<Alarm> alarms = new();

    }
}
