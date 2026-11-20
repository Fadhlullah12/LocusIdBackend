namespace LocusIDBackend.Models.Entities
{
    public class AcademicSession : BaseEntity
    {
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}