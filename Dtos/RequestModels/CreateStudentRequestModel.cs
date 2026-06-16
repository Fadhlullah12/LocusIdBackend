namespace LocusIDBackend.Dtos.RequestModels
{
    public class CreateStudentRequestModel
    {
        public string MatricNumber { get; set; } = default!;
        public string Department { get; set; } = default!;
        public string Faculty { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string SchoolName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string? PhotoUrl { get; set; }   
    }
}