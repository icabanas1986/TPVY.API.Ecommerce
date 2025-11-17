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

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserDto dto)
        {
            var result = await _authService.ActualizaUsuario(dto.Id,dto.Nombre, dto.Email,dto.RolId, dto.Password);
            return Ok(new { message = result });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authService.EliminaUsuario(id);
            if (!result)
                return NotFound(new { message = "Usuario no encontrado" });
            return Ok(new { message = "Usuario eliminado correctamente" });
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.ObtenerUsuarios();
            return Ok(users);
        }

        // DTOs
        public record UserDto(int Id, string Nombre, string Email, int RolId,string Password);
        public record RegisterDto(string Nombre, string Email, string Password, int RolId);
        public record LoginDto(string Email, string Password);
    }
}
