
using LocusIDBackend.Models.Entities;

namespace LocusIDBackend.Repositories.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<bool> HasReadReceiptAsync(string messageId, string studentId);
        Task AddReadReceiptAsync(MessageReadReceipts receipt);
        Task<List<Message>> GetMessagesForCoursesAsync(List<string> courseIds);
    }
}