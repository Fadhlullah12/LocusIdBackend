using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department> Get(string id);
        Task<Department> Get(Expression<Func<Department, bool>> expression);
        Task<ICollection<Department>> GetDepartmentsByIds(ICollection<string> departmentIds);
        Task<ICollection<Department>> GetDepartmentsByIds(Expression<Func<Department, bool>> expression);

    }
}