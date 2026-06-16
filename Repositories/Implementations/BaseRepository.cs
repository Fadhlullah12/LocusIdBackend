using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Repositories.Interfaces;

namespace LocusIDBackend.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public async Task<T?> GetById(string id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        public async Task<bool> Delete(string id)
        {
            var entity = await GetById(id);
            if (entity == null) return false;

            _context.Remove(entity);
            return true;
        }
        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
        public void DeleteRange(IEnumerable<T> entities)
        {

            _context.RemoveRange(entities);
        }
    }
}
