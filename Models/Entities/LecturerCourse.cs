namespace LocusIDBackend.Models.Entities
{
    public class LecturerCourse : BaseEntity
    {
        public string LecturerId { get; set; } = default!;
        public Lecturer Lecturer{ get; set; } = default!;
        public string CourseId { get; set; } = default!;
        public Course Course { get; set; } = default!;
    }
}