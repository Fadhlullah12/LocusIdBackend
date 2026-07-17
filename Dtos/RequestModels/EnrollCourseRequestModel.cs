namespace LocusIDBackend.Dtos.RequestModels
{
    public class EnrollCourseRequestModel
    {
        public string CourseName { get; set; } = default!;
        public string CourseCode { get; set; } = default!; 
    }
}