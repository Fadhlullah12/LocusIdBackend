using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Context;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;

namespace LocusIDBackend.Repositories.Implementations
{
    public class StudentSessionRepository : BaseRepository<StudentSession>, IStudentSessionRepository
    {
        public StudentSessionRepository(ApplicationContext context) : base(context) { }

        public async Task<IEnumerable<StudentSession>> GetSessionsByStudent(string studentId)
        {
            return await _context.Set<StudentSession>().Where(ss => ss.StudentId == studentId).ToListAsync();
        }

        public async Task<IEnumerable<StudentSession>> GetStudentsBySession(string sessionId)
        {
            return await _context.Set<StudentSession>().Where(ss => ss.SessionId == sessionId).ToListAsync();
        }

        public async Task<StudentSession> GetByStudentAndSession(string studentId, string sessionId)
        {
            return await _context.Set<StudentSession>().FirstOrDefaultAsync(ss => ss.StudentId == studentId && ss.SessionId == sessionId) 
                ?? throw new InvalidOperationException($"StudentSession not found for student {studentId} and session {sessionId}");
        }
    }
}
