namespace LocusIDBackend.Models.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string FacultyId { get; set; } = default!;
        public Faculty Faculty { get; set; } = default!;
        public ICollection<Student>? Students { get; set; } = new HashSet<Student>();
        public ICollection<Lecturer>? Lecturers { get; set; } = new HashSet<Lecturer>();
        public ICollection<DepartmentCourse>? DepartmentCourses { get; set;} = new HashSet<DepartmentCourse>();
    }
}