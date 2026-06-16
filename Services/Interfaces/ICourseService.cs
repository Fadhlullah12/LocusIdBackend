using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.ResponseModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface ICourseService
    {
        Task<BaseResponse<ICollection<CourseDto>>> GetCourses();
        Task<BaseResponse<ICollection<SessionDto>>> CourseSessions(string courseId);
        Task<BaseResponse<ICollection<StudentDto>>> GetCourseStudents(string courseId);
        Task<BaseResponse<CourseDto>> CreateCourse(CreateCourseRequestModel model,string token);
        Task<BaseResponse<ICollection<CourseAttendanceDto>>> CourseAttendance(string courseCode,string token);
    }
}