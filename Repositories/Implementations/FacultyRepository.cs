
using System.Linq.Expressions;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocusIDBackend.Repositories.Implementations
{
    public class FacultyRepository : BaseRepository<Faculty>, IFacultyRepository
    {
        public FacultyRepository(ApplicationContext context) : base(context) { }
        public async Task<Faculty> Get(string id)
        {
            var faculty = await _context.Faculties
           .FirstOrDefaultAsync(a => a.Id == id);
           return faculty!;
        }

        public async Task<Faculty> Get(Expression<Func<Faculty, bool>> expression)
        {
             var faculty = await _context.Faculties
            .Include(d => d.Departments)
            .FirstOrDefaultAsync(expression);
           return faculty!;
        }

        public async Task<ICollection<Faculty>> GetFacultysByIds(ICollection<string> FacultyIds)
        {
             var faculty = await _context.Faculties
           .Where(a => FacultyIds.Contains(a.Id))
           .ToListAsync();
           return faculty!;
        }

        public async Task<ICollection<Faculty>> GetFaculties(Expression<Func<Faculty, bool>> expression)
        {
              var faculty = await _context.Faculties
           .Where(expression)
           .ToListAsync();
           return faculty!;
        }
    }
}