namespace LocusIDBackend.Dtos.RequestModels
{
    public class CreateSessionRequestModel
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public string CourseCode { get; set; } = default!;
        public int Duration { get; set; } = default!;
        //public string Token { get; set; } = default!;  
    }
}