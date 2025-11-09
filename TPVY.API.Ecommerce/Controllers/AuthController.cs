using Microsoft.AspNetCore.Mvc;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto.Nombre, dto.Email, dto.Password, dto.RolId);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Email, dto.Password);
            return Ok(new { token });
        }

        // DTOs
        public record RegisterDto(string Nombre, string Email, string Password, int RolId);
        public record LoginDto(string Email, string Password);
    }
}
