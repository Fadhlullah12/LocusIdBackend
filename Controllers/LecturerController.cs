using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/lecturer")]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLecturerRequestModel model)
        {
            var result = await _lecturerService.CreateLecturer(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("courses")]

        public async Task<IActionResult> GetAll()
        {
           string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var result = await _lecturerService.GetLecturerCourses(rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}