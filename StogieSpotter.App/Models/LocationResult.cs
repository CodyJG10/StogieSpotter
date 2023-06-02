using GoogleApi.Entities.Places.Photos.Response;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.Models
{
    public class LocationResult
    {
        public NearByResult Place { get; set; }
        public IEnumerable<ImageSource> Photos { get; set; }
        public ImageSource Icon { get; set; }
        public string RatingText { get; set; }
        public string PriceText { get; set; }
        public string Distance { get; set; }
    }
}