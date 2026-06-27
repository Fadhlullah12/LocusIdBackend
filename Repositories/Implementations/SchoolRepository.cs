using System.Linq.Expressions;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocusIDBackend.Repositories.Implementations
{
    public class SchoolRepository : BaseRepository<School>, ISchoolRepository
    {
        public SchoolRepository(ApplicationContext context) : base(context) { }
        public async Task<School> Get(string id)
        {
           var school = await _context.Schools
           .Include(s => s.Faculties)
           .ThenInclude(f => f.Departments)
           .FirstOrDefaultAsync(a => a.Id == id);
           
           return school!;
        }

        public async Task<School> Get(Expression<Func<School, bool>> expression)
        {
            var school = await _context.Schools
            .Include(s => s.Faculties)
            .ThenInclude(f => f.Departments)
           .FirstOrDefaultAsync(expression);
           return school!;
        }

        public async Task<ICollection<School>> GetSchools(Expression<Func<School, bool>> expression)
        {
            var school = await _context.Schools
           .Where(expression)
           .ToListAsync();
           return school!;
        }

        public async Task<ICollection<School>> GetSchoolsByIds(ICollection<string> SchoolIds)
        {
            var school = await _context.Schools
           .Where(a => SchoolIds.Contains(a.Id))
           .ToListAsync();
           return school!;
        }
    }
}