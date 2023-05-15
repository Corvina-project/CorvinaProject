using LiveChartsCore.Defaults;
using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiAuth0App.Views;

public partial class GraphTagPage : ContentPage {
	public GraphTagPage(ObservableCollection<Tag> tags) {
		GraphTagPageViewModel.Data = new ObservableCollection<DateTimePoint>(tags.Select(t => {
			var splitted = t.tagValue.Split("\n");
			var data = DateTime.ParseExact(splitted[0].Replace("Data: ", ""), "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None);
			var valore = double.Parse(splitted[1].Replace("Valore: ", ""));

			return new DateTimePoint(data, valore);
		}));
		InitializeComponent();
	}
}