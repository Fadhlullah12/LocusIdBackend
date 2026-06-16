namespace LocusIDBackend.Models.Entities
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public string LecturerId { get; set; } = default!;
        public Lecturer Lecturer{ get; set; } = default!;
        public ICollection<Session>? Sessions { get; set;} = new HashSet<Session>();
        public ICollection<StudentCourse>? CourseStudents{ get; set;} = new HashSet<StudentCourse>();
        public ICollection<DepartmentCourse>? CourseDepartments { get; set;} = new HashSet<DepartmentCourse>();
    }
}
