using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IAcademicSessionService
    {
        Task<BaseResponse<AcademicSessionDto>> CreateAcademicSession(CreateAcademicSessionRequestModel request,string token);
        Task<BaseResponse<string>> DeleteAcademicSession(string academicSessionId, string token);
    }
}