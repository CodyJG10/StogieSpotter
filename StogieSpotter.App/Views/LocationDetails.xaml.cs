using CommunityToolkit.Mvvm.ComponentModel;
using StogieSpotter.App.Models;
using StogieSpotter.App.ViewModels;

namespace StogieSpotter.App.Views;

[QueryProperty(nameof(PlaceId), "PlaceId")]
public partial class LocationDetails : ContentPage
{
	public string PlaceId 
	{
		set
		{
			(BindingContext as LocationDetailsViewModel).Init(value);
			OnPropertyChanged();
		}
	}

	public LocationDetails()
	{
		InitializeComponent();
		BindingContext = new LocationDetailsViewModel();
	}
}