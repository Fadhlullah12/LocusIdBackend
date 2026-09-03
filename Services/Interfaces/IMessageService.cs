using LocusIDBackend.Dtos;
using LocusIDBackend.Dtos.RequestModels;

namespace LocusIDBackend.Services.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageAsync(string lecturerId, string courseId, MessageDto dto);
        Task<List<NotificationDto>> GetNotificationsForStudentAsync(string studentId);
        Task MarkAsReadAsync(string messageId, string studentId);
    }
}