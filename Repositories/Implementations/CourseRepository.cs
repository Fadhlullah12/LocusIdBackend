using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationContext context) : base(context) { }

        public async Task<Course> Get(Expression<Func<Course, bool>> expression)
        {
            var course = await _context.Set<Course>()
            .Include(c => c.CourseStudents)
            .Include(c => c.Sessions)!
            .ThenInclude(s => s.Attendances)
            .FirstOrDefaultAsync(expression);
            return course!;
        }

        public async Task<ICollection<Course>> GetCoursesByIds(ICollection<string> courseIds)
        {
            var courses = await _context.Courses
            .Include(c => c.Sessions)
            .Where(c => courseIds.Contains(c.CourseCode))
            .ToListAsync();
            return courses;
        }
    }
}

               
