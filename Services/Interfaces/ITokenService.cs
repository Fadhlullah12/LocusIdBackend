namespace LocusIDBackend.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string role, string FullName);
    }
}
