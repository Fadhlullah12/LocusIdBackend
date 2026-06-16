using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/faculty")]
    public class FacultyController : ControllerBase
    {
        IFacultyService _facultyService;
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateFaculty(CreateFacultyRequestModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var response = await _facultyService.CreateFaculty(request, token);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetFaculties(string schoolName)
        {
            var response = await _facultyService.GetFaculties(schoolName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}