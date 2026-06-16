using Microsoft.AspNetCore.Mvc;
using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/school")]
    public class SchoolController : ControllerBase
    {
        ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateSchool(CreateSchoolRequestModel request)
        {
            var response = await _schoolService.CreateSchool(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetSchools()
        {
            var response = await _schoolService.GetSchools();
            return Ok(response);
        }
    }
}