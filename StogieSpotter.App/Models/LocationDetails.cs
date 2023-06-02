using GoogleApi.Entities.Places.Details.Response;
using StogieSpotter.PlacesApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.Models
{
    public class LocationDetails
    {
        public DetailsResult Details { get; set; }
        public IEnumerable<ImageSource> Photos { get; set; }
        public string PhoneNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public string Address { get; set; }
        public string RatingText { get; set; }
        public string PriceText { get; set; }
    }
}