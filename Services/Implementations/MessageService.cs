using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Models.Entities;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using LocusIDBackend.Hubs;
namespace LocusIDBackend.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IStudentCourseRepository _studentCourseRepo;
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageService(IMessageRepository messageRepo, IStudentCourseRepository studentCourseRepo, IHubContext<MessageHub> hubContext)
        {
            _messageRepo = messageRepo;
            _studentCourseRepo = studentCourseRepo;
            _hubContext = hubContext;
        }

        public async Task SendMessageAsync(string lecturerId, string courseId, MessageDto dto)
        {
            var message = new Message
            {
                LecturerId = lecturerId,
                CourseId = courseId,
                Content = dto.Content
            };

            await _messageRepo.Create(message);
            await _messageRepo.Save();

            // send to SignalR group for the course
            await _hubContext.Clients.Group($"course-{courseId}")
                .SendAsync("ReceiveMessage", new
                {
                    message.Id,
                    message.Content,
                    message.CreatedAt,
                    message.CourseId
                });
        }

        public async Task<List<NotificationDto>> GetNotificationsForStudentAsync(string studentId)
        {
            var studentCourses = await _studentCourseRepo.GetAll(sc => sc.StudentId == studentId);
            var courseIds = studentCourses.Select(sc => sc.CourseId).ToList();
            var messages = await _messageRepo.GetMessagesForCoursesAsync(courseIds);

            var result = new List<NotificationDto>();
            foreach (var m in messages)
            {
                result.Add(new NotificationDto
                {
                    Id = m.Id,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt,
                    CourseId = m.CourseId,
                    CourseName = m.Course.CourseName ?? string.Empty,
                    LecturerName = $"{m.Lecturer?.User?.FirstName} {m.Lecturer?.User?.LastName}",
                    IsRead = await _messageRepo.HasReadReceiptAsync(m.Id, studentId)
                });
            }
            return result;
        }

        public async Task MarkAsReadAsync(string messageId, string studentId)
        {
            if (!await _messageRepo.HasReadReceiptAsync(messageId, studentId))
            {
                var receipt = new MessageReadReceipts
                {
                    MessageId = messageId,
                    StudentId = studentId
                };
                await _messageRepo.AddReadReceiptAsync(receipt);
                await _messageRepo.Save();
            }
        }
    }
}