namespace LocusIDBackend.Models.Entities
{
    public class Attendance : BaseEntity
    {
        public bool Attended { get; set; }
        public string StudentId { get; set; } = default!;
        public Student Student { get; set; } = default!;
        public string SessionId { get; set; } = default!;
        public Session Session { get; set; } = default!;
    }

}
