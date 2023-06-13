using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.PlacesApi.Models
{
    public class NearByResult : BaseResult
    {
        /// <summary>
        /// Vicinity contains a feature name of a nearby location.
        /// Often this feature refers to a street or neighborhood within the given results.
        /// The vicinity property is only returned for a Nearby Search.
        /// </summary>
        public virtual string Vicinity { get; set; }
    }
}
