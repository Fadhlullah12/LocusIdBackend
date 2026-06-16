using System.Linq.Expressions;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocusIDBackend.Repositories.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationContext context) : base(context) { }

        public async Task<Department> Get(string id)
        {
           var department  = await _context.Departments
           .FirstOrDefaultAsync(a => a.Id == id);
           return department!;
        }

        public async Task<Department> Get(Expression<Func<Department, bool>> expression)
        {
            var department = await _context.Departments
           .FirstOrDefaultAsync(expression);
           return department!;
        }

        public async Task<ICollection<Department>> GetDepartmentsByIds(ICollection<string> departmentIds)
        {
             var department = await _context.Departments
           .Where(a => departmentIds.Contains(a.Id))
           .ToListAsync();
           return department!;
        }

        public async Task<ICollection<Department>> GetDepartmentsByIds(Expression<Func<Department, bool>> expression)
        {
             var department = await _context.Departments
           .Where(expression)
           .ToListAsync();
           return department!;
        }
    }
}