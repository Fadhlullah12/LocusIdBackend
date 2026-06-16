namespace LocusIDBackend.Services.Interfaces
{
    public interface IGeoService
    {
        public bool IsWithinRange(double userLat, double userLon, double targetLat, double targetLon, double rangeInMeters);
    }
}