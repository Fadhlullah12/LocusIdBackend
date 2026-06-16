using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface ILecturerService
    {
        Task<BaseResponse<CreateLecturerResponseDto>> CreateLecturer(CreateLecturerRequestModel request);
        Task<BaseResponse<ICollection<CourseDto>>> GetLecturerCourses(string token);
    }
}