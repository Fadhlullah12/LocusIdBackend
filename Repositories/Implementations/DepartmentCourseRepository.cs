using System.Linq.Expressions;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocusIDBackend.Repositories.Implementations
{
    public class DepartmentCourseRepository : BaseRepository<DepartmentCourse>, IDepartmentCourseRepository
    {
       public DepartmentCourseRepository(ApplicationContext context) : base(context) { }

        public async Task<DepartmentCourse> Get(Expression<Func<DepartmentCourse, bool>> expression)
        {
             var departmentCourse  = await _context.DepartmentCourses
           .FirstOrDefaultAsync(expression);
           return departmentCourse!;
        }

        public async Task<DepartmentCourse> Get(string Id)
        {
             var departmentCourse  = await _context.DepartmentCourses
           .FirstOrDefaultAsync(a => a.Id == Id);
           return departmentCourse!;
        }

        public async Task<ICollection<DepartmentCourse>> GetCoursesByIds(ICollection<string> CourseIds)
        {
            var departmentCourse  = await _context.DepartmentCourses
           .Where(a => CourseIds.Contains(a.Id))
           .ToListAsync();
           return departmentCourse!;
        }

        public async Task<ICollection<DepartmentCourse>> GetCoursesByIds(Expression<Func<DepartmentCourse, bool>> expression)
        {
             var departmentCourse  = await _context.DepartmentCourses
           .Where(expression)
           .ToListAsync();
           return departmentCourse!;
        }
    }
}