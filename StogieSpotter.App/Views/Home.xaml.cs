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
		StartAnimations();
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var placesService = (GooglePlacesService)MyServiceLocator.Services.GetService(typeof(GooglePlacesService));
        this.ShowPopup(new SearchForCity(new SearchForCityViewModel(placesService, BindingContext as HomeViewModel)));
    }

    private async void StartAnimations()
	{
		List<VisualElement> visualElements = new List<VisualElement>();
		visualElements.AddRange(new VisualElement[] { CitySettingsGrid, RangeSlider, RangeLabel });
		visualElements.ForEach(x => x.IsVisible = false);
		foreach (var element in visualElements)
		{
			element.IsVisible = true;

            var originalTranslationX = element.TranslationX;
            var originalTranslationY = element.TranslationY;

            // Set the element's initial translation for the animation
            element.TranslationY = originalTranslationY + 100;

            // Create and start the animation
            await element.TranslateTo(originalTranslationX, originalTranslationY, 700, Easing.CubicInOut);

            // Reset the element's translation to the original values
            element.TranslationX = originalTranslationX;
            element.TranslationY = originalTranslationY;
        }
	}
}