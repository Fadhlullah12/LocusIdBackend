
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
            var response = await _academicSessionService.CreateAcademicSession(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        } 
    }
}