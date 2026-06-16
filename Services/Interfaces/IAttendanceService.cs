using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task<BaseResponse<AttendanceDto>> MarkAttendance(MarkAttendanceRequestModel request, string token);
        Task<BaseResponse<int>> CheckAttendanceEligibility(CheckAttendanceEligibilityRequestModel model);
    }
}