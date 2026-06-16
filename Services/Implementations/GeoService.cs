using LocusIDBackend.Services.Interfaces;
using System;

namespace LocusIDBackend.Services.Implementations
{
    public class GeoService : IGeoService
    {
        private const double EarthRadiusMeters = 6371000;
        private const double DegreeToRadian = Math.PI / 180.0;

        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            
            if (Math.Abs(lat1 - lat2) < 1e-7 && Math.Abs(lon1 - lon2) < 1e-7)
                return 0;

            var dLat = (lat2 - lat1) * DegreeToRadian;
            var dLon = (lon2 - lon1) * DegreeToRadian;

            var lat1Rad = lat1 * DegreeToRadian;
            var lat2Rad = lat2 * DegreeToRadian;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

           
            var c = 2 * Math.Asin(Math.Sqrt(a));

            return EarthRadiusMeters * c;
        }

        public bool IsWithinRange(double userLat, double userLon, double targetLat, double targetLon, double rangeInMeters)
        {
            // 2. The "Buffer" Logic: GPS on phones is rarely 100% accurate. 
            // In a classroom, a student might be "2 meters outside" due to signal drift.
            const double GpsGraceBuffer = 5.0; 
            
            var distance = CalculateDistance(userLat, userLon, targetLat, targetLon);
            
            return distance <= (rangeInMeters + GpsGraceBuffer);
        }
    }
}