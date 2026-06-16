
using System.Linq.Expressions;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocusIDBackend.Repositories.Implementations
{
    public class DirectorRepository : BaseRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(ApplicationContext context) : base(context) { }
        public async Task<Director> Get(string id)
        {
            var director = await _context.Directors.
            FirstOrDefaultAsync(d => d.Id == id);
            return director!;
        }

        public async Task<Director> Get(Expression<Func<Director, bool>> expression)
        {
            var director = await _context.Directors.
            Include(a => a.School).
            FirstOrDefaultAsync(expression);
            return director!;
        }

        public async Task<ICollection<Director>> GetDepartmentsByIds(ICollection<string> departmentIds)
        {
            var director = await _context.Directors.
            Where(d => departmentIds.Contains(d.Id))
            .ToListAsync();
            return director!;
        }

        public async Task<ICollection<Director>> GetDepartmentsByIds(Expression<Func<Director, bool>> expression)
        {
            var director = await _context.Directors.
            Where(expression)
            .ToListAsync();
            return director!;
        }
    }
}