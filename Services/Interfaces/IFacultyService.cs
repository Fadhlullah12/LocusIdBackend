using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IFacultyService
    {
        Task<BaseResponse<FacultyDto>> CreateFaculty(CreateFacultyRequestModel request, string token);
        Task<BaseResponse<ICollection<FacultyDto>>> GetFaculties(string schoolName);
    }
}