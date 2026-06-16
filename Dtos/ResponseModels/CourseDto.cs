namespace LocusIDBackend.Dtos.ResponseModels
{
    public class CourseDto
    {
        public string CourseName { get; set; } = default!;
        public string CourseCode { get; set; } = default!; 
        public DateTime CreatedAt { get; set; } = default!; 
    }
}
