using LocusIDBackend.Dtos.RequestModels;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(CreateDepartmentRequestModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var response = await _departmentService.CreateDepartment(request, token);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
            [HttpGet("faculty/{facultyName}")]
            public async Task<IActionResult> GetDepartmentsByFaculty(string facultyName)
            {
                var response = await _departmentService.GetDepartmentsByFaculty(facultyName);
                if (!response.Success)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
    }
}
