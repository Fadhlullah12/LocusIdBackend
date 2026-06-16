using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/course")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCourseRequestModel model)
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var response = await _courseService.CreateCourse(model,rawToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok (response);
        }
        [HttpGet("students")]
        public async Task<IActionResult> GetStudents([FromQuery] string courseCode)
        {
            
            var response = await _courseService.GetCourseStudents(courseCode);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("sessions")]
        public async Task<IActionResult> GetSessions([FromQuery] string courseCode)
        {
            
            var response = await _courseService.CourseSessions(courseCode);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var response = await _courseService.GetCourses();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
         [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendances(string courseCode)
        {
             string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();
            var response = await _courseService.CourseAttendance(courseCode,rawToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}