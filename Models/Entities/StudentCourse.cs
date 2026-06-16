namespace LocusIDBackend.Models.Entities
{
    public class StudentCourse : BaseEntity
    {
        public string StudentId { get; set; } = default!;
        public Student Student{ get; set; } = default!;
        public string CourseId { get; set;} = default!;
        public Course Course { get; set; } = default!;
    }
}