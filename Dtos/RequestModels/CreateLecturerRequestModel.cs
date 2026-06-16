namespace LocusIDBackend.Dtos.RequestModels
{
    public class CreateLecturerRequestModel
    {
        public string LecturerId { get; set; } = default!;
        public string SchoolName { get; set; } = default!;
        public string Department { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public IFormFile? PhotoUrl { get; set; }
    }
}