using System.Linq.Expressions;
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<Session> Get(Expression<Func<Session, bool>> expression);
        Task<Session> GetId(string Id);
        Task<ICollection<Session>> GetAll(Expression<Func<Session, bool>> expression);
    }
}
