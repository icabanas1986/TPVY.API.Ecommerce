using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;
        public CategoriasController(ICategoriaService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.ObtenerCategorias());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _service.ObtenerPorId(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [Route("Crear")]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            await _service.CrearCategoria(categoria);
            return Ok(new { message = "Categoría creada correctamente" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, Categoria categoria)
        {
            if (id != categoria.Id) return BadRequest();
            await _service.ActualizarCategoria(categoria);
            return Ok(new { message = "Categoría actualizada" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarCategoria(id);
            return Ok(new { message = "Categoría eliminada" });
        }
    }
}
