using System.Device.Location;

namespace DeliveryServiceApi.Services
{
    public class DistanceService
    {
        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var coord1 = new GeoCoordinate(lat1, lon1);
            var coord2 = new GeoCoordinate(lat2, lon2);
            return coord1.GetDistanceTo(coord2) / 1000; // Distance in kilometers
        }

        public double CalculateDeliveryCharge(double distance)
        {
            
            return distance * 1.0;
        }
    }
}

