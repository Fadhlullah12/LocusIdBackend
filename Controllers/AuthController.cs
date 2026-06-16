using LocusIDBackend.Dtos;
using LocusIDBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocusIDBackend.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet] 
        public IActionResult Get() => Ok();
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.Login(request);
            return result.Success ? Ok(result) : Unauthorized(result);
        }
    }
}
