using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace LocusIDBackend.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) { }

        public async Task<User> Get(Expression<Func<User, bool>> expression)
        {
            var user = await _context.Set<User>()
            .Include(u => u.Student)
            .Include(u => u.Lecturer)
            .FirstOrDefaultAsync(expression);
            return user!;
        }

        
    }
}
