using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface ISessionService
    {
        Task<BaseResponse<SessionDto>> CreateSession(CreateSessionRequestModel request, string token);
        Task<BaseResponse<SessionStudentDto>> GetSessionStudents(string sessionId);
    }
}