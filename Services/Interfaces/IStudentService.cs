using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IStudentService
    {
        Task<BaseResponse<StudentDto>> CreateStudent(CreateStudentRequestModel request);
        Task<BaseResponse<ICollection<CourseDto>>> EnrollCourse(ICollection<EnrollCourseRequestModel> model, string token);
        Task<BaseResponse<ICollection<CourseDto>>> GetStudentCourses(string token);
        Task<BaseResponse<string>> DropCourses(ICollection<string> courseIds, string token);
    }
}