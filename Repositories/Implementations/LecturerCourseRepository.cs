using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;

namespace LocusIDBackend.Repositories.Implementations
{
    public class LecturerCourseRepository : BaseRepository<LecturerCourse>, ILecturerCourseRepository
    {
        public LecturerCourseRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<LecturerCourse>> GetCoursesByLecturer(string lecturerId)
        {
            return await _context.Set<LecturerCourse>()
            .Where(lc => lc.LecturerId == lecturerId).ToListAsync();
        }

        public async Task<IEnumerable<LecturerCourse>> GetLecturersByCourse(string courseId)
        {
            return await _context.Set<LecturerCourse>()
            .Where(lc => lc.CourseId == courseId).ToListAsync();
        }

        public async Task<LecturerCourse?> GetByLecturerAndCourse(string lecturerId, string courseId)
        {
            return await _context.Set<LecturerCourse>()
            .FirstOrDefaultAsync(lc => lc.LecturerId == lecturerId && lc.CourseId == courseId);
        }
    }
}
