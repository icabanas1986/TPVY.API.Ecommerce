using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPVY.API.Ecommerce.DTOs.Rol;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Services;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolService.ObtenerTodosAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var rol = await _rolService.ObtenerPorIdAsync(id);
            if (rol == null)
                return NotFound();

            return Ok(rol);
        }

        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult> Create([FromBody] RegistrerRolDTO dto)
        {
            var nuevoRol = await _rolService.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new {id=nuevoRol.Id},nuevoRol);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRolDTO dto)
        {
            if (id != dto.IdRol)
                return BadRequest();

            await _rolService.ActualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rolService.EliminarAsync(id);
            return NoContent();
        }
    }
}
