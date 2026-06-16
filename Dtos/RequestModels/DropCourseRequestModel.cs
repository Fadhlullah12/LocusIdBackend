namespace LocusIDBackend.Dtos.RequestModels
{
    public class DropCourseRequestModel
    {
        public ICollection<string> CourseIds { get; set; } = new HashSet<string>();
        public string Token { get; set; } = default!;        
    }
}