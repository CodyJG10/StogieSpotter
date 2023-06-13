using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using StogieSpotter.PlacesApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.PlacesApi
{
    public class BasePlaceService : IPlacesService
    {
        protected string _apiKey;

        public BasePlaceService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public virtual Task<List<string>> GetAutocompleteResults(Coordinate origin, string query)
        {
            return null;
        }

        public virtual Task<int> GetDistance(Coordinate origin, Coordinate destination)
        {
            return null;
        }

        public virtual Task<PlacesNearbySearchResponse> GetNearbyPlaces(Coordinate origin, string keyword, int radius)
        {
            return null;
        }

        public virtual Task<PlacesNearbySearchResponse> GetNearbyPlaces(string origin, string keyword, int radius)
        {
            return null;
        }

        public virtual Task<Uri> GetPhoto(string photoRef, int size)
        {
            return null;
        }

        public virtual Task<DetailsResult> GetPlaceDetails(string placeId)
        {
            return null;
        }

        public virtual Task<string> ReverseGeocode(Coordinate location)
        {
            return null;
        }

        public virtual async Task<Coordinate> Geocode(string query) 
        {
            return null;
        }
    }
}