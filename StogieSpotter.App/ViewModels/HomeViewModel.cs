using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Photos.Response;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.VisualBasic;
using StogieSpotter.App.Models;
using StogieSpotter.App.Services;
using StogieSpotter.App.Views;
using StogieSpotter.PlacesApi;
using StogieSpotter.PlacesApi.Interfaces;
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
        [NotifyPropertyChangedFor(nameof(RadiusText))]
        private double _radius = 10;

        public string RadiusText
        {
            get
            {
                return ((int)Math.Round(Radius)).ToString();
            }
        }

        private IPlacesService _placesService;

        [ObservableProperty]
        private bool _loading = true;

        [ObservableProperty]
        private string _query = "TAP TO SEARCH ANOTHER AREA";

        private bool _isUsingPhysicalLocation = true;

        [ObservableProperty]
        private bool _noResults = false;

        [ObservableProperty]
        private bool _showPlaces = false;

        public HomeViewModel(IPlacesService placesService)
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

        private async Task Init()
        {
            await CheckAndRequestLocationPermission();

            Loading = true;
            ShowPlaces = false;
            NoResults = false;
            if (NearbyPlaces != null)
            {
                NearbyPlaces = new ObservableCollection<LocationResult>();
            }
            Coordinate locationOrigin = null;
            if(!_isUsingPhysicalLocation)
                locationOrigin = await _placesService.Geocode(Query);


            PlacesNearbySearchResponse nearbyResults;
            var radius = (int)Math.Round(Radius);
            if (_isUsingPhysicalLocation)
                nearbyResults = await _placesService.GetNearbyPlaces((await MyServiceLocator.Services.GetService<LocationService>().GetDeviceLocation()),"Cigar Lounge", radius);
            else
                nearbyResults = await _placesService.GetNearbyPlaces(Query, "Cigar Lounge", radius);
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

                    ImageSource icon = null;
                    if (place.IconUrl != null)
                        icon = ImageSource.FromUri(new Uri(place.IconUrl));

                    LocationResult result = new LocationResult()
                    {
                        Photos = new List<ImageSource>(),
                        Place = place,
                        Icon = icon,
                        RatingText = starsText,
                        PriceText = priceText,
                    };
                    List<ImageSource> photos = new List<ImageSource>();
                    var details = await _placesService.GetPlaceDetails(place.PlaceId);
                    if (details.Photos != null)
                    {
                        foreach (var photoRef in details.Photos.Take(3))
                        {
                            var photo = await _placesService.GetPhoto(photoRef.PhotoReference, 400);
                            photos.Add(ImageSource.FromUri(photo));
                        }
                    }
                    result.Photos = photos;

                    // Distance
                    int distance;
                    var destination = await _placesService.Geocode(details.FormattedAddress);
                    if (_isUsingPhysicalLocation)
                    {
                        distance = await _placesService.GetDistance((await MyServiceLocator.Services.GetService<LocationService>().GetDeviceLocation()), destination);
                    }
                    else
                    {
                        distance = await _placesService.GetDistance(locationOrigin, destination);
                    }
                    result.Distance = distance.ToString();
                    NearbyPlaces.Add(result);
                }
                catch (Exception e) 
                {
                    Console.WriteLine(e.Message);
                }
            }
            NearbyPlaces = new ObservableCollection<LocationResult>(NearbyPlaces.OrderBy(obj => int.Parse(obj.Distance)));
            Loading = false;
            if (NearbyPlaces.Count == 0)
            {
                NoResults = true;
                ShowPlaces = false;
            }
            else
            {
                NoResults = false;
                ShowPlaces = true;
            }
            OnPropertyChanged(nameof(NearbyPlaces));
        }

        public async Task CheckAndRequestLocationPermission()
        {
            if(Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                await Shell.Current.DisplayAlert("Permission is required", "Location permission is required for Stogie Spotter to work", "OK");
            }
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
                await CheckAndRequestLocationPermission();
        }

        [RelayCommand]
        public async void LocationClicked(LocationResult location)
        {
            await Shell.Current.GoToAsync($"//home/details?PlaceId={location.Place.PlaceId}");
        }

        [RelayCommand]
        public async void RadiusUpdated()
        {
            await Init();
            var filtered = from location in NearbyPlaces
                           where int.Parse(location.Distance) <= Radius
                           select location;
            NearbyPlaces = new ObservableCollection<LocationResult>(filtered);
        }
    }
}