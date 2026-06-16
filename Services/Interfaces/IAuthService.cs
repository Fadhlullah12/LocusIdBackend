using LocusIDBackend.Dtos;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IAuthService
    {
        Task<BaseResponse<LoginResponseDto>> Login(LoginRequestDto request);
    }
}
