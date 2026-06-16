namespace LocusIDBackend.Dtos.RequestModels
{
    public class CheckAttendanceEligibilityRequestModel
    {
        public string Token { get; set; } = default!;
        public string CourseId { get; set; } = default!;
    }
}