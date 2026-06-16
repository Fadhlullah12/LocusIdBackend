using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LocusIDBackend.Services.Interfaces;


namespace LocusIDBackend.Services.Implementations
{
    public class DecodeTokenService : IDecodeTokenService
    {
        public string GetIdFromRawToken(string token)
        {
            ArgumentNullException.ThrowIfNull(token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var idClaim = jwtToken.Claims.FirstOrDefault(q => q.Type == ClaimTypes.NameIdentifier);

            return idClaim?.Value;
        }
    }
}