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

namespace StogieSpotter.App.ViewModels
{
    public partial class LocationDetailsViewModel : ObservableObject
    {
        private GooglePlacesService _placesService;

        [ObservableProperty]
        private LocationDetails _details;
        [ObservableProperty]
        private bool _loading = true;
        [ObservableProperty]
        private ImageSource _coverImage;

        public LocationDetailsViewModel()
        {
            _placesService = MyServiceLocator.Services.GetService<GooglePlacesService>();
        }

        public async void Init(string placeId)
        {
            var place = await _placesService.GetPlaceDetails(placeId);

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

            string priceText = "";
            if (place.PriceLevel != 0)
            {
                var priceLevel = (int)place.PriceLevel;
                for (int i = 0; i < priceLevel; i++)
                {
                    priceText += "$";
                }
                switch(priceLevel)
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

            string websiteUrl = place.Website;
                
            if (websiteUrl.EndsWith("/"))
            {
                websiteUrl = websiteUrl.TrimEnd('/');
            }

            LocationDetails details = new LocationDetails()
            {
                Photos = new List<ImageSource>(),
                Details = place,
                RatingText = starsText,
                PriceText = priceText,
                Address = place.FormattedAddress,
                PhoneNumber = place.FormattedPhoneNumber,
                WebsiteUrl = websiteUrl.Replace("https://", "").Replace("www.", "").Replace("http://", ""),
            };
            List<ImageSource> photos = new List<ImageSource>();
            foreach (var photoRef in place.Photos)
            {
                var photo = await _placesService.GetPhoto(photoRef.PhotoReference, 400);
                photos.Add(ImageSource.FromStream(() => photo.Stream));
            }
            var coverImage = await _placesService.GetPhoto(place.Photos.ToList()[0].PhotoReference, 600);
            CoverImage = ImageSource.FromStream(() => coverImage.Stream);
            details.Photos = photos;
            Details = details;
            Loading = false;
            OnPropertyChanged();
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
    }
}