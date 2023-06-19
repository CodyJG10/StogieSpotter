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

        public async Task<double> GetDistance(Coordinate origin, Coordinate destination)
        {
            double distance = 0;
            var destinationCoords = new Microsoft.Maui.Devices.Sensors.Location(destination.Latitude, destination.Longitude);
            if (origin == null)
            {
                var currentLocation = await Geolocation.Default.GetLocationAsync();
                distance = currentLocation.CalculateDistance(destinationCoords, DistanceUnits.Miles);
            }
            else
            {
                distance = (new Microsoft.Maui.Devices.Sensors.Location(origin.Latitude, origin.Longitude).CalculateDistance(destinationCoords, DistanceUnits.Miles));
            }
            return distance;
        }
    }

    public class DistanceCalculator
    {
        private const double EarthRadiusKm = 6371.0;

        public double CalculateDistance(Coordinate start, Coordinate end)
        {
            var dLat = DegreeToRadian(end.Latitude - start.Latitude);
            var dLon = DegreeToRadian(end.Longitude - start.Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreeToRadian(start.Latitude)) * Math.Cos(DegreeToRadian(end.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadiusKm * c;
            return distance;
        }

        private double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }
    }
}
