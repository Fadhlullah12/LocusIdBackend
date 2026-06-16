using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }
        [HttpPost]
        public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceRequestModel request)
        {
            string authHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { status = "error", message = "No valid token found in header" });
            }
            string rawToken = authHeader.Substring("Bearer ".Length).Trim();

            var result = await _attendanceService.MarkAttendance(request, rawToken);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("eligibility")]
        public async Task<IActionResult> CheckEligibility([FromBody] CheckAttendanceEligibilityRequestModel model)
        {
            var result = await _attendanceService.CheckAttendanceEligibility(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}