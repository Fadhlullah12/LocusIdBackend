using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> Create(T entity);
        Task<T?> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Update(T entity);
        Task<bool> Delete(string id);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> Save();
    }
}