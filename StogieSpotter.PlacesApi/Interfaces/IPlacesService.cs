using GoogleApi.Entities.Common;
using GoogleApi.Entities.Places.Details.Response;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.PlacesApi.Interfaces
{
    public interface IPlacesService
    {
        public Task<PlacesNearbySearchResponse> GetNearbyPlaces(Coordinate origin, string keyword, int radius);
        public Task<PlacesNearbySearchResponse> GetNearbyPlaces(string origin, string keyword, int radius);
        public Task<DetailsResult> GetPlaceDetails(string placeId);
        public Task<string> ReverseGeocode(Coordinate location);
        public Task<Uri> GetPhoto(string photoRef, int size);
        public Task<List<string>> GetAutocompleteResults(Coordinate origin, string query);
        public Task<int> GetDistance(Coordinate origin, Coordinate destination);
        public Task<Coordinate> Geocode(string query);
    }
}
