using GoogleApi;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Maps.AddressValidation.Response;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using GoogleApi.Entities.Maps.Geocoding.Location.Request;
using GoogleApi.Entities.Maps.Geolocation.Request;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Photos.Request;
using GoogleApi.Entities.Places.Photos.Response;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using Newtonsoft.Json;
using static GoogleApi.GooglePlaces;
using static GoogleApi.GooglePlaces.Search;

namespace StogieSpotter.PlacesApi
{
    public class GooglePlacesService
    {
        private string _apiKey;
        private Coordinate? _currentLocation;

        public GooglePlacesService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<Coordinate> GetLocationAsync()
        {
            GeolocationRequest request = new GeolocationRequest()
            {
                Key = _apiKey,
            };

            var result = await GoogleMaps.Geolocation.QueryAsync(request);
            return result.Location;
        }

        public async Task<PlacesNearbySearchResponse> GetNearbyPlaces(string keyword, int miles)
        {
            var location = await GetLocationAsync();
            double radius = miles * 1609.34;
            var request = new PlacesNearBySearchRequest()
            {
                Key = _apiKey,
                Location = location,
                Keyword = keyword,
                Radius = radius, 
            };

            var result = await new NearBySearchApi().QueryAsync(request);

            return result;
        }

        public async Task<PlacesNearbySearchResponse> GetNearbyPlaces(string location, string keyword, int miles)
        {
            double radius = 1609.34;
            var request = new PlacesNearBySearchRequest()
            {
                Key = _apiKey,
                Location = await Geocode(location),
                Keyword = keyword,
                Radius = radius
            };

            var result = await new NearBySearchApi().QueryAsync(request);

            return result;
        }

        public async Task<DetailsResult> GetPlaceDetails(string placeId)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={_apiKey}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RootObject>(content);
            return result.Result;
        }

        public async Task<string> ReverseGeocode(Coordinate coords)
        {
            LocationGeocodeRequest request = new LocationGeocodeRequest()
            {
                Key = _apiKey,
                Location = coords
            };

            var result = await GoogleMaps.Geocode.LocationGeocode.QueryAsync(request);
            return result.Results.ToList()[0].FormattedAddress;
        }

        public async Task<Coordinate> Geocode(string query)
        {
            AddressGeocodeRequest request = new AddressGeocodeRequest()
            {
                Key = _apiKey,
                Address = query
            };

            var result = await GoogleMaps.Geocode.AddressGeocode.QueryAsync(request);
            var firstResult = result.Results.FirstOrDefault();
            var placeId = firstResult.PlaceId;
            

            var details = await GetPlaceDetails(placeId);
            return new Coordinate(details.Geometry.Location.Lat, details.Geometry.Location.Lng);
        }

        public async Task<PlacesPhotosResponse> GetPhoto(string photoRef, int maxWidth)
        {
            var photoRequest = new PlacesPhotosRequest()
            {
                Key = _apiKey,
                PhotoReference = photoRef,
                MaxWidth = maxWidth
            };
            var photoResponse = await Photos.QueryAsync(photoRequest);
            return photoResponse;
        }

        public async Task<List<string>> GetAutocompleteResults(string query)
        {
            if (_currentLocation == null)
                _currentLocation = await GetLocationAsync();
            var autocompleteRequest = new PlacesAutoCompleteRequest()
            {
                Key = _apiKey,
                Input = query,
                LocationBias = new GoogleApi.Entities.Places.Common.LocationBias()
                {
                    Location = _currentLocation,
                    Radius = 100 * 1609.34,
                },
                RestrictType = GoogleApi.Entities.Places.AutoComplete.Request.Enums.RestrictPlaceType.Cities,
                Origin = _currentLocation,
                Location = _currentLocation,
            };

            var autocompleteResponse = await GooglePlaces.AutoComplete.QueryAsync(autocompleteRequest);
            var predictions = autocompleteResponse.Predictions;
            return (from x in predictions
                    select x.Description).ToList();
        }

        public async Task<int> GetDistance(Coordinate origin, Coordinate destination)
        {
            List<LocationEx> originList = new List<LocationEx>()
            {
                new LocationEx(new CoordinateEx(origin.Latitude, origin.Longitude))
            };
            List<LocationEx> destinationList = new List<LocationEx>()
            {
                new LocationEx(new CoordinateEx(destination.Latitude, destination.Longitude))
            };
            var request = new DistanceMatrixRequest()
            {
                Key = _apiKey,
                Origins = originList,
                Destinations = destinationList,
            };

            var response = await GoogleMaps.DistanceMatrix.QueryAsync(request);
            var distance = response.Rows.ToList()[0].Elements.ToList()[0].Distance.Value;
            return (int)Math.Round(distance / 1609.34);
        }

        public async Task<int> GetDistance(Coordinate destination)
        {
            if (_currentLocation == null)
                _currentLocation = await GetLocationAsync();
            List<LocationEx> originList = new List<LocationEx>()
            {
                new LocationEx(new CoordinateEx(_currentLocation.Latitude, _currentLocation.Longitude))
            };
            List<LocationEx> destinationList = new List<LocationEx>()
            {
                new LocationEx(new CoordinateEx(destination.Latitude, destination.Longitude))
            };
            var request = new DistanceMatrixRequest()
            {
                Key = _apiKey,
                Origins = originList,
                Destinations = destinationList,
            };

            var response = await GoogleMaps.DistanceMatrix.QueryAsync(request);
            var distance = response.Rows.ToList()[0].Elements.ToList()[0].Distance.Value;
            return (int)Math.Round(distance / 1609.34);
        }
    }
}