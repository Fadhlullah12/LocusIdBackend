
namespace LocusIDBackend.Dtos.ResponseModels
{
    public class CourseAttendanceDto
    {
        public string DateCreated { get; set; } = default!;
        public bool Status { get; set; } = default!;
        public string TimeMarked { get; set; } = default!;
    }
}