using GoogleApi.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.Services
{
    public class LocationService
    {
        public async Task<Coordinate> GetDeviceLocation()
        {
            var location = await Geolocation.Default.GetLocationAsync();
            return new Coordinate(location.Latitude, location.Longitude);
        }
    }
}
