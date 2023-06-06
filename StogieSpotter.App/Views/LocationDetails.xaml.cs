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

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //      await MainImage.FadeTo(0, 250); 
        //(BindingContext as LocationDetailsViewModel).PhotoSelectedCommand.Execute((sender as Image).Source);
        //      await MainImage.FadeTo(1, 250); 
        var viewModel = BindingContext as LocationDetailsViewModel;
        var selectedImageSource = (sender as Image).Source;

        await MainImage.FadeTo(0, 250);

        viewModel.PhotoSelectedCommand.Execute(selectedImageSource);

        await MainImage.FadeTo(1, 250);


    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        var displayInfo = DeviceDisplay.MainDisplayInfo;
        var deviceWidth = displayInfo.Width;
        var deviceHeight = displayInfo.Height;

        // Determine the device size based on width or height
        var deviceSize = Math.Min(deviceWidth, deviceHeight);

        var viewModel = BindingContext as LocationDetailsViewModel;

        // Set the label style based on the device size
        if (deviceSize >= 600)
        {
            viewModel.LabelStyle = "LargeLabelStyle";
        }
        else if (deviceSize >= 400)
        {
            viewModel.LabelStyle = "MediumLabelStyle";
        }
        else
        {
            viewModel.LabelStyle = "SmallLabelStyle";
        }
    }

}