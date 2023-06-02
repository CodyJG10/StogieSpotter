using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Photos.Response;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using StogieSpotter.App.Models;
using StogieSpotter.App.Views;
using StogieSpotter.PlacesApi;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LocationResult> _nearbyPlaces = new ObservableCollection<LocationResult>();

        [ObservableProperty]
        private int _radius = 10;

        private GooglePlacesService _placesService;

        [ObservableProperty]
        private bool _loading = true;

        [ObservableProperty]
        private string _query = "TAP TO SEARCH ANOTHER AREA";

        private bool _isUsingPhysicalLocation = true;

        public HomeViewModel(GooglePlacesService placesService)
        {
            _placesService = placesService;
            Init();
        }

        [RelayCommand]
        public void CitySelected(string city)
        {
            Query = city;
            if(city == null || city.Length == 0)
            {
                _isUsingPhysicalLocation = true;
                Query = "TAP TO SEARCH ANOTHER AREA";
            }
            else
            {
                _isUsingPhysicalLocation = false;
            }
            Init();
        }

        private async void Init()
        {
            Loading = true;
            if (NearbyPlaces != null)
            {
                NearbyPlaces = new ObservableCollection<LocationResult>();
            }
            Coordinate locationOrigin = null;
            if(!_isUsingPhysicalLocation)
                locationOrigin = await _placesService.Geocode(Query);


            PlacesNearbySearchResponse nearbyResults;
            if(_isUsingPhysicalLocation)
                nearbyResults = await _placesService.GetNearbyPlaces("Cigar Lounge", Radius);
            else
                nearbyResults = await _placesService.GetNearbyPlaces(Query, "Cigar Lounge", Radius);
            foreach (var place in nearbyResults.Results)
            {
                try
                {
                    string starsText = "";
                    if (place.Rating.HasValue)
                    {
                        int stars = (int)Math.Floor(place.Rating.Value);
                        for (int i = 0; i < stars; i++)
                        {
                            starsText += "★";
                        }
                    }

                    string priceText = "";
                    if(place.PriceLevel.HasValue)
                    {
                        var priceLevel = (int)place.PriceLevel.Value;
                        for(int i = 0; i < priceLevel; i++)
                        {
                            priceText += "$";
                        }
                    }

                    LocationResult result = new LocationResult()
                    {
                        Photos = new List<ImageSource>(),
                        Place = place,
                        Icon = ImageSource.FromUri(new Uri(place.IconUrl)),
                        RatingText = starsText,
                        PriceText = priceText,
                    };
                    List<ImageSource> photos = new List<ImageSource>();
                    var details = await _placesService.GetPlaceDetails(result.Place.PlaceId);
                    foreach (var photoRef in details.Photos.Take(3))
                    {
                        var photo = await _placesService.GetPhoto(photoRef.PhotoReference, 400);
                        photos.Add(ImageSource.FromStream(() => photo.Stream));
                    }
                    result.Photos = photos;

                    // Distance
                    int distance;
                    var destination = await _placesService.Geocode(details.FormattedAddress);
                    if (_isUsingPhysicalLocation)
                    {
                        distance = await _placesService.GetDistance(destination);
                    }
                    else
                    {
                        distance = await _placesService.GetDistance(locationOrigin, destination);
                    }
                    result.Distance = distance.ToString();
                    NearbyPlaces.Add(result);
                }
                catch (Exception) { }
            }
            NearbyPlaces = new ObservableCollection<LocationResult>(NearbyPlaces.OrderBy(obj => int.Parse(obj.Distance)));
            RadiusUpdated();
            Loading = false;
            OnPropertyChanged(nameof(NearbyPlaces));
        }

        [RelayCommand]
        public async void LocationClicked(LocationResult location)
        {
            await Shell.Current.GoToAsync($"//home/details?PlaceId={location.Place.PlaceId}");
        }

        [RelayCommand]
        public void RadiusUpdated()
        {
            var filtered = from location in NearbyPlaces
                           where int.Parse(location.Distance) <= Radius
                           select location;
            NearbyPlaces = new ObservableCollection<LocationResult>(filtered);
        }
    }
}