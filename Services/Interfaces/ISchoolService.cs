using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;
namespace LocusIDBackend.Services.Interfaces
{
    public interface ISchoolService
    {
        Task<BaseResponse<SchoolDto>> CreateSchool(CreateSchoolRequestModel request);
        Task<BaseResponse<ICollection<SchoolDto>>> GetSchools();
    }
}