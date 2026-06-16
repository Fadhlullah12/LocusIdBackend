namespace LocusIDBackend.Models.Entities
{
    public class Session : BaseEntity
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public bool IsActive { get; set; } = true;
        public int Duration { get; set; } = default!;
        public DateTime? EndTime { get; set; }
        public string CourseId { get; set; } = default!;
        public Course Course{ get; set; } = default!;
        public string LecturerId { get; set; } = default!; 
        public Lecturer Lecturer { get; set; } = default!;
        public string AcademicSessionId { get; set; } = default!;
        public AcademicSession AcademicSession { get; set; } = default!;
        public ICollection<Attendance>? Attendances { get; set; } = new HashSet<Attendance>();
        public ICollection<StudentSession>? StudentSessions { get; set;} = new HashSet<StudentSession>();
    }
}
