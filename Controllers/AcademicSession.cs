
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/academic-sessions")]
    public class AcademicSession : ControllerBase
    {
        IAcademicSessionService _academicSessionService;
        public AcademicSession(IAcademicSessionService academicSessionService)
        {
            _academicSessionService = academicSessionService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAcademicSession(CreateAcademicSessionRequestModel request)
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();
            var response = await _academicSessionService.CreateAcademicSession(request,rawToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        } 
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string academicSessionId)
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var response = await _academicSessionService.DeleteAcademicSession(academicSessionId, rawToken);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}