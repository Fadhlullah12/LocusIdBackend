namespace LocusIDBackend.Dtos.ResponseModels
{
    public class CreateLecturerResponseDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string? PhotoUrl { get; set; }   
    }
}