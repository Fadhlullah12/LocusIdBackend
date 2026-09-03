using System.Security.Claims;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILecturerCourseRepository _lecturerCourseRepo;
        private readonly ILecturerRepository _lecturerRepo;
        private readonly IStudentRepository _studentRepo;

        public MessagesController(IMessageService messageService,
            ILecturerCourseRepository lecturerCourseRepo,
            ILecturerRepository lecturerRepo,
            IStudentRepository studentRepo)
        {
            _messageService = messageService;
            _lecturerCourseRepo = lecturerCourseRepo;
            _lecturerRepo = lecturerRepo;
            _studentRepo = studentRepo;
        }

        // Lecturer sends message to their course
        [HttpPost("courses/{courseId}")]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> SendToCourse(string courseId, [FromBody] MessageDto dto)
        {
            // Get logged-in user's id from JWT claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Resolve Lecturer entity by UserId
            var lecturer = await _lecturerRepo.Get(l => l.UserId == userId);
            if (lecturer == null) return Forbid();

            // Ensure lecturer teaches this course
            var assignment = await _lecturerCourseRepo.GetByLecturerAndCourse(lecturer.Id, courseId);
            if (assignment == null) return Forbid();

            await _messageService.SendMessageAsync(lecturer.Id, courseId, dto);
            return CreatedAtAction(nameof(SendToCourse), new { courseId = courseId }, null);
        }

        // Student fetches notifications
        [HttpGet("notifications")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var student = await _studentRepo.Get(s => s.UserId == userId);
            if (student == null) return NotFound();

            var notifications = await _messageService.GetNotificationsForStudentAsync(student.Id);
            return Ok(notifications);
        }

        // Student marks a message as read
        [HttpPost("{messageId}/read")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MarkAsRead(string messageId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var student = await _studentRepo.Get(s => s.UserId == userId);
            if (student == null) return NotFound();

            await _messageService.MarkAsReadAsync(messageId, student.Id);
            return NoContent();
        }
    }
}
