namespace LocusIDBackend.Dtos
{
    public class LoginResponseDto
    {
        public string UserId { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
