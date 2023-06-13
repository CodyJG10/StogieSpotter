using GoogleApi.Entities.Places;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StogieSpotter.PlacesApi.Models
{
    public class PlacesNearbySearchResponse : BasePlacesResponse
    {
        /// <summary>
        /// Contains an array of places, with information about each. See Search Results for information about these results.
        /// The Places API returns up to 20 establishment results per query. Additionally, political results may be returned
        /// which serve to identify the area of the request.
        /// </summary>
        public virtual IEnumerable<NearByResult> Results { get; set; }

        /// <summary>
        /// Contains a token that can be used to return up to 20 additional results. A next_page_token will not be returned if there are no additional results to display.
        /// The maximum number of results that can be returned is 60. There is a short delay between when a next_page_token is issued, and when it will become valid.
        /// </summary>
        [JsonProperty("next_page_token")]
        public virtual string NextPageToken { get; set; }
    }
}
