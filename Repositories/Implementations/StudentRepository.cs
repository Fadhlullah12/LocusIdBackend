using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationContext context) : base(context) { }

        public async Task<Student> Get(Expression<Func<Student, bool>> expression)
        {
             var student = await _context.Set<Student>()
            .Include(a => a.User)
            .Include(a => a.StudentCourses)!
            .ThenInclude(sc => sc.Course)
            .FirstOrDefaultAsync(expression);
            return student!;
        }

        public async Task<ICollection<Student>> GetAll(Expression<Func<Student, bool>> expression)
        {
             var student = await _context.Set<Student>()
            .Include(a => a.User)
            .Where(expression)
            .ToListAsync();
            return student;
        }

            public async Task<Student> GetId(string Id)
            {
                var student = await _context.Set<Student>()
                .Include(a => a.User)
                .FirstOrDefaultAsync(s => s.Id == Id);
                return student!;
            }
    }
}
