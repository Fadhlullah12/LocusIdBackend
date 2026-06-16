namespace LocusIDBackend.Dtos.RequestModels
{
    public class MarkAttendanceRequestModel
    {
        public string CourseCode { get; set; } = default!;
        public double Latitude { get; set; } = default!;
        public double Longitude { get; set; } = default!;
    }
}