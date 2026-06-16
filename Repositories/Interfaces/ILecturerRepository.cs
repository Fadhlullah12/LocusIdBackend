using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface ILecturerRepository : IBaseRepository<Lecturer>
    {
        Task<Lecturer> Get(Expression<Func<Lecturer, bool>> expression);
    }
}
