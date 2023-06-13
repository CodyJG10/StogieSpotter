﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StogieSpotter.PlacesApi.Models.Geocoding
{
    public class GeocodeResponse
    {
        /// <summary>
        /// Results.
        /// When the geocoder returns results, it places them within a (JSON) results array.
        /// Even if the geocoder returns no results (such as if the address doesn't exist) it still returns an empty results array.
        /// </summary>
        [JsonPropertyName("results")]
        public virtual IEnumerable<Result> Results { get; set; }
    }
}
