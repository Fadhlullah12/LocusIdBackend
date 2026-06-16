namespace LocusIDBackend.Dtos.RequestModels
{
    public class CreateCourseRequestModel
    {
        public string CourseName { get; set; } = default!;
        public string CourseCode { get; set; } = default!; 
    }
}