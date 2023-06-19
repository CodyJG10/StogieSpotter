using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.AutoComplete.Response;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using static GoogleApi.GooglePlaces.Search;
using GoogleApi.Entities.Maps.Geocoding.Location.Request;
using GoogleApi;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using GoogleApi.Entities.Places.Photos.Response;
using GoogleApi.Entities.Places.Photos.Request;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.DistanceMatrix.Request;
using System.Text.Json;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Common.Converters;
using System.Collections.Specialized;
using static GoogleApi.GooglePlaces;
using static GoogleApi.GoogleMaps;
using GoogleApi.Interfaces.Maps.Geocode;
using static GoogleApi.GoogleMaps.Geocode;

namespace StogieSpotter.PlacesApi
{
    public class PlaceService : BasePlaceService
    {
        public PlaceService(string apiKey) : base(apiKey) { }

        public override async Task<PlacesNearbySearchResponse> GetNearbyPlaces(Coordinate location, string keyword, int miles)
        {
            double radius = miles * 1609.34;
            var request = new PlacesNearBySearchRequest()
            {
                Key = _apiKey,
                Location = location,
                Keyword = keyword,
                Radius = radius,
            };

            var result = await new NearBySearchApi(new HttpClient()).QueryAsync(request);

            return result;
        }

        public override async Task<PlacesNearbySearchResponse> GetNearbyPlaces(string query, string keyword, int miles)
        {
            var location = await Geocode(query);
            return await GetNearbyPlaces(location, keyword, miles);
        }

        public override async Task<Coordinate> Geocode(string query)
        {
            AddressGeocodeRequest request = new AddressGeocodeRequest()
            {
                Key = _apiKey,
                Address = query
            };

            var response = await new AddressGeocodeApi(new HttpClient()).QueryAsync(request);
            return response.Results.ToList()[0].Geometry.Location;
        }

        public override async Task<Uri> GetPhoto(string photoRef, int maxWidth)
        {
            var photoRequest = new PlacesPhotosRequest()
            {
                Key = _apiKey,
                PhotoReference = photoRef,
                MaxWidth = maxWidth
            };

            var uri = photoRequest.GetUri();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri);
            var url = response.RequestMessage.RequestUri;
            return url;
        }

        public override async Task<DetailsResult> GetPlaceDetails(string placeId)
        {
            var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={_apiKey}";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RootObject>(content);
            return result.Result;

            //PlacesDetailsRequest request = new PlacesDetailsRequest()
            //{
            //    Key = _apiKey,
            //    PlaceId = placeId
            //};

            //var result = await new DetailsApi(new HttpClient()).QueryAsync(request);
            //return result.Result;
        }

        public override async Task<string> ReverseGeocode(Coordinate coords)
        {
            LocationGeocodeRequest request = new LocationGeocodeRequest()
            {
                Key = _apiKey,
                Location = coords
            };

            var result = await new LocationGeocodeApi(new HttpClient()).QueryAsync(request);
            return result.Results.ToList()[0].FormattedAddress;
        }

        public override async Task<List<string>> GetAutocompleteResults(Coordinate origin, string query)
        {
            var autocompleteRequest = new PlacesAutoCompleteRequest()
            {
                Key = _apiKey,
                Input = query,
                LocationBias = new GoogleApi.Entities.Places.Common.LocationBias()
                {
                    Location = origin,
                    Radius = 100 * 1609.34,
                },
                RestrictType = GoogleApi.Entities.Places.AutoComplete.Request.Enums.RestrictPlaceType.Cities,
                Origin = origin,
                Location = origin,
            };

            var autocompleteResponse = await new AutoCompleteApi(new HttpClient()).QueryAsync(autocompleteRequest);
            var predictions = autocompleteResponse.Predictions;
            return (from x in predictions
                    select x.Description).ToList();
        }

        public override async Task<int> GetDistance(Coordinate origin, Coordinate destination)
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

            var response = await new DistanceMatrixApi(new HttpClient()).QueryAsync(request);
            var distance = response.Rows.ToList()[0].Elements.ToList()[0].Distance.Value;
            return (int)Math.Round(distance / 1609.34);
        }
    }
}