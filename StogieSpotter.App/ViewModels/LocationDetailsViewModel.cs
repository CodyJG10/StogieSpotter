using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StogieSpotter.App.Models;
using StogieSpotter.PlacesApi;
using StogieSpotter.App.Services;
using GoogleApi.Entities.Maps.Common;
using CommunityToolkit.Mvvm.Input;
using StogieSpotter.PlacesApi.Interfaces;
using GoogleApi.Entities.Places.Details.Response;

namespace StogieSpotter.App.ViewModels
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        private IPlacesService _placesService;

        [ObservableProperty]
        private LocationDetails _details;
        [ObservableProperty]
        private bool _loading = true;
        [ObservableProperty]
        private ImageSource _coverImage;
        [ObservableProperty]
        private double _pageOpacity;
        [ObservableProperty]
        private string _labelStyle;

        public LocationDetailsViewModel()
        {
            _placesService = MyServiceLocator.Services.GetService<IPlacesService>();
        }

        public async void Init(string placeId)
        {
            var place = await _placesService.GetPlaceDetails(placeId);

            string websiteUrl = place.Website;

            if (websiteUrl != null)
            {
                if (websiteUrl.EndsWith("/"))
                {
                    websiteUrl = websiteUrl.TrimEnd('/');
                    websiteUrl = websiteUrl.Replace("https://", "").Replace("www.", "").Replace("http://", "");
                }
            }

            string starsText = GetStarRating(place);
            var priceText = GetPriceText(place);

            LocationDetails details = new LocationDetails()
            {
                Photos = new List<ImageSource>(),
                Details = place,
                RatingText = starsText,
                PriceText = priceText,
                Address = place.FormattedAddress,
                PhoneNumber = place.FormattedPhoneNumber,
                WebsiteUrl = websiteUrl
            };

            List<ImageSource> photos = new List<ImageSource>();
            if (place.Photos != null)
            {
                foreach (var photoRef in place.Photos)
                {
                    var photo = await _placesService.GetPhoto(photoRef.PhotoReference, 400);
                    photos.Add(ImageSource.FromUri(photo));
                }
            }
            Uri coverImage = new Uri("https://via.placeholder.com/300x200?text=NO+IMAGE+FOUND");
            var image  = await _placesService.GetPhoto(place.Photos.ToList()[0].PhotoReference, 600);
            if(image != null)
            {
                coverImage = image;
            }
            CoverImage = ImageSource.FromUri(coverImage);
            details.Photos = photos;
            Details = details;
            Loading = false;
            OnPropertyChanged();
            FadeInBackground();
        }

        private string GetPriceText(DetailsResult place)
        {
            string priceText = "";
            if (place.PriceLevel != 0)
            {
                var priceLevel = (int)place.PriceLevel;
                for (int i = 0; i < priceLevel; i++)
                {
                    priceText += "$";
                }
                switch (priceLevel)
                {
                    case 1:
                        priceText += " - Cheap";
                        break;
                    case 2:
                        priceText += " - Moderate";
                        break;
                    case 3:
                        priceText += " - A Little High";
                        break;
                    case 4:
                        priceText += " - Expensive";
                        break;
                }
            }
            else
            {
                priceText = "No prices found";
            }
            return priceText;
        }

        private string GetStarRating(DetailsResult place)
        {
            string starsText = "";
            if (place.Rating != 0)
            {
                int stars = (int)Math.Floor(place.Rating);
                for (int i = 0; i < stars; i++)
                {
                    starsText += "★";
                }
            }
            else
            {
                starsText = "No reviews found";
            }
            return starsText;
        }

        [RelayCommand]
        public void PhotoSelected(ImageSource image)
        {
            CoverImage = image;
        }

        [RelayCommand]
        public async void GetDirections()
        {
            var destination = await _placesService.Geocode(Details.Address);
            var options = new MapLaunchOptions { Name = "Destination Name" };
            await Map.OpenAsync(new Microsoft.Maui.Devices.Sensors.Location(destination.Latitude, destination.Longitude), options);
        }

        [RelayCommand]
        public void Call()
        {
            if (PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open(Details.PhoneNumber);
        }

        [RelayCommand]
        public async void GoToWebsite()
        {
            Uri uri = new Uri(Details.Details.Website);
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private async void FadeInBackground()
        {
            double value = 0;
            double step = 1 / (250 / 16.67); // Assuming 60 FPS (16.67ms per frame)

            while (value < 1)
            {
                PageOpacity = value;
                await Task.Delay(16); // Delay for one frame (16ms)
                value += step;
            }

            PageOpacity = 1; // Ensure
        }
    }
}