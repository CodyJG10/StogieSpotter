using CommunityToolkit.Maui.Views;
using StogieSpotter.App.Models;
using StogieSpotter.App.Services;
using StogieSpotter.App.ViewModels;
using StogieSpotter.PlacesApi;

namespace StogieSpotter.App.Views;

public partial class Home : ContentPage
{
	public Home(HomeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("//home/details");
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var placesService = (GooglePlacesService)MyServiceLocator.Services.GetService(typeof(GooglePlacesService));
        this.ShowPopup(new SearchForCity(new SearchForCityViewModel(placesService, BindingContext as HomeViewModel)));
    }


    private void NearbyPlacesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = nearbyPlacesList.SelectedItem;
        (BindingContext as HomeViewModel).LocationClickedCommand.Execute(selected);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var selected = nearbyPlacesList.SelectedItem;
        (BindingContext as HomeViewModel).LocationClickedCommand.Execute(selected);
    }
}