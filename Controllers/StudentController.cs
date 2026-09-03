using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentRequestModel model)
        {
            var result = await _studentService.CreateStudent(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] ICollection<EnrollCourseRequestModel> model)
        {
             string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _studentService.EnrollCourse(model, rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var result = await _studentService.GetStudentCourses(rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string studentId)
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var result = await _studentService.DeleteStudent(studentId, rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}