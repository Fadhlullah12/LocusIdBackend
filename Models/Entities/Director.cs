namespace LocusIDBackend.Models.Entities
{
    public class Director : BaseEntity
    {
        public string StaffId { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public User User  { get; set; } = default!;
        public School School { get; set; } = default!;
    }
}