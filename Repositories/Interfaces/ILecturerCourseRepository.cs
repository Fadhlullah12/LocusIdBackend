using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface ILecturerCourseRepository : IBaseRepository<LecturerCourse>
    {
        Task<IEnumerable<LecturerCourse>> GetCoursesByLecturer(string lecturerId);
        Task<IEnumerable<LecturerCourse>> GetLecturersByCourse(string courseId);
        Task<LecturerCourse?> GetByLecturerAndCourse(string lecturerId, string courseId);
    }
}
