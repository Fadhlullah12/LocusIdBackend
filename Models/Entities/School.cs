namespace LocusIDBackend.Models.Entities
{
    public class School : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string DirectorId { get; set; } = default!;
        public Director Director { get; set; } = default!;
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public ICollection<Faculty> Faculties { get; set; } = new HashSet<Faculty>();
        public ICollection<Lecturer> Lecturers { get; set; } = new HashSet<Lecturer>();
        public ICollection<AcademicSession> AcademicSessions { get; set; } = new HashSet<AcademicSession>();
    }
}