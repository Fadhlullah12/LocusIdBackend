namespace LocusIDBackend.Models.Entities
{
    public class Lecturer : BaseEntity
    {
        public string StaffId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public string DepartmentId { get; set; } = default!;
        public Department Department { get; set; } = default!;
        public string SchoolId { get; set; } = default!;
        public School School { get; set; } = default!;
        public ICollection<Session>? Sessions { get; set; } = new HashSet<Session>();
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}