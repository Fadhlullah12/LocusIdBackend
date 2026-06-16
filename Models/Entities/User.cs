namespace LocusIDBackend.Models.Entities
{
    public class User : BaseEntity
    {

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public Student? Student { get; set; }
        public Lecturer? Lecturer { get; set; }
        public Director? Director { get; set; }
    }
}