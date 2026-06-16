namespace LocusIDBackend.Models.Entities
{
    public class DepartmentCourse : BaseEntity
    {
        public string DepartmentId { get; set; } = default!;
        public Department Department { get; set;} = default!;
        public string CourseId { get; set;} = default!;
        public Course Course { get; set; } = default!;

    }
}