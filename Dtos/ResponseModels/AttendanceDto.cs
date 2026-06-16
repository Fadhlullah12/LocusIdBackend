namespace LocusIDBackend.Dtos.ResponseModels
{
    public class AttendanceDto
    {
    public string StudentId { get; set; } = default!;
    public string StudentName { get; set; } = default!;
    public string CourseTitle { get; set; } = default!;
    public string SessionId { get; set; } = default!;
    }
}