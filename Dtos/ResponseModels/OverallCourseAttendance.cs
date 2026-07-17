
namespace LocusIDBackend.Dtos.ResponseModels
{
    public class OverallCourseAttendance
    {
        public int AttendancePercentage { get; set; } = default!;
        public int AttendedClasses { get; set; } = default!;

        public ICollection<CourseAttendanceDto> Attendances { get; set; } = default!;
    }
}