using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        [HttpPost]
          public async Task<IActionResult> Create([FromBody]CreateSessionRequestModel model)
        {
             string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _sessionService.CreateSession(model, rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> SessionStudents(string sessionId)
        {
            var result = await _sessionService.GetSessionStudents(sessionId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string sessionId)
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var result = await _sessionService.DeleteSession(sessionId, rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}