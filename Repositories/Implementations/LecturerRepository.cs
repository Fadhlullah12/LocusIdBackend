using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class LecturerRepository : BaseRepository<Lecturer>, ILecturerRepository
    {
        public LecturerRepository(ApplicationContext context) : base(context) { }

        public async Task<Lecturer> Get(Expression<Func<Lecturer, bool>> expression)
        {
            var lecturer = await _context.Lecturers
            .Include(l => l.Courses)
            .Include(l => l.User)
            .FirstOrDefaultAsync(expression);
            return lecturer!;
        }
    }
}
