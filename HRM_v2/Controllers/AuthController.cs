using Microsoft.AspNetCore.Mvc;
using HRM_v2.DTOs;
using HRM_v2.Services.Interfaces;
namespace HRM_v2.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var result = _authService.Login(dto);
            return Ok(result);
        }
    }
}
