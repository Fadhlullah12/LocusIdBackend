namespace LocusIDBackend.Models.Entities
{
    public class Student : BaseEntity
    {
        public string MatricNumber { get; set; } = default!;
        public string Faculty { get; set; } = default!;
        public string UserId { get; set; } = default!; 
        public User User { get; set; } = default!;
        public string DepartmentId { get; set; } = default!;
        public Department Department { get; set; } = default!;
        public string SchoolId { get; set; } = default!;
        public School School { get; set; } = default!;
        public ICollection<Attendance>? Attendances { get; set;} = new HashSet<Attendance>();
        public ICollection<StudentCourse>? StudentCourses { get; set;} = new HashSet<StudentCourse>();
        public ICollection<StudentSession>? StudentSessions { get; set;} = new HashSet<StudentSession>();
    }
}