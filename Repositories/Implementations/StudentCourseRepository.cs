using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class StudentCourseRepository : BaseRepository<StudentCourse>, IStudentCourseRepository
    {
        public StudentCourseRepository(ApplicationContext context) : base(context) { }

        public async Task<ICollection<StudentCourse>> GetAll(Expression<Func<StudentCourse, bool>> expression)
        {
            var studentCourses = await _context.Set<StudentCourse>()
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .Include(sc => sc.Course)
                .Where(expression)
                .ToListAsync();

            return studentCourses;
        }
    }
}
