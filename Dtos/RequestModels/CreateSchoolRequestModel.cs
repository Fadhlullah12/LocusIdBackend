namespace LocusIDBackend.Dtos.RequestModels
{
    public class CreateSchoolRequestModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string StaffId { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhotoUrl { get; set; }
        public string SchoolName { get; set; } = default!;
    }
}