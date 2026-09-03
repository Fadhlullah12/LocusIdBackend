using Microsoft.EntityFrameworkCore;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Context;

namespace LocusIDBackend.Repositories.Implementations
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
                    public MessageRepository(ApplicationContext context) : base(context) { }

        // public MessageRepository(ApplicationContext context)
        // {
        //     _context = context;
        // }

        // public async Task AddMessageAsync(Message message)
        // {
        //     _context.Messages.Add(message);
        //     await _context.SaveChangesAsync();
        // }

        public async Task<List<Message>> GetMessagesForCoursesAsync(List<string> courseIds)
        {
            return await _context.Messages
                .Where(m => courseIds.Contains(m.CourseId))
                .OrderByDescending(m => m.CreatedAt)
                .Include(m => m.Course)
                .Include(m => m.Lecturer).ThenInclude(l => l.User)
                .ToListAsync();
        }

        // public async Task<bool> HasReadReceiptAsync(string messageId, string studentId)
        // {
        //     return await _context.Set<MessageReadReceipts>()
        //         .AnyAsync(r => r.MessageId == messageId && r.StudentId == studentId);
        // }

        // public async Task AddReadReceiptAsync(MessageReadReceipts receipt)
        // {
        //     _context.Set<MessageReadReceipts>().Add(receipt);
        // }
        public async Task AddReadReceiptAsync(MessageReadReceipts receipt)
        {
            await _context.Set<MessageReadReceipts>().AddAsync(receipt);
        }

        public async Task<bool> HasReadReceiptAsync(string messageId, string studentId)
        {
            return await _context.Set<MessageReadReceipts>()
                .AnyAsync(r => r.MessageId == messageId && r.StudentId == studentId);
        }

    }
}