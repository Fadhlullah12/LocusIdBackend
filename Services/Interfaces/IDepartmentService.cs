using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseResponse<DepartmentDto>> CreateDepartment(CreateDepartmentRequestModel request, string token); 
        Task<BaseResponse<ICollection<FacultyDto>>> GetDepartmentsByFaculty(string facultyName);
    }
}