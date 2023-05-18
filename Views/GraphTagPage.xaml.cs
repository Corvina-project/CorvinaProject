using LiveChartsCore.Defaults;
using MauiAuth0App.Models;
using MauiAuth0App.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MauiAuth0App.Views;

public partial class GraphTagPage : ContentPage {
	public GraphTagPage(Tag tag) {
		GraphTagPageViewModel.Data = new ObservableCollection<DateTimePoint>(tag.Dati.Select(pair => {
			return new DateTimePoint(pair.Key, pair.Value);
		}));
			
		/*
		var list = new ObservableCollection<DateTimePoint>();
		var random = new Random();
		for (int i = 0; i < 20; i++)
			list.Add(new DateTimePoint(new DateTime(2012, 8, 15, 5, 10, 4).AddDays(i), i));

		list.Add(new DateTimePoint(new DateTime(0), 0));
        GraphTagPageViewModel.Data = list;
			*/

        InitializeComponent();
	}
}