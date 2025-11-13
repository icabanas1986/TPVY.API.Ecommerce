using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPVY.API.Ecommerce.DTOs.Producto;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _productoService.ObtenerTodosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Create([FromBody] RegistrerProductoDTO producto)
        {
            var nuevoProducto = await _productoService.CrearAsync(producto);
            return CreatedAtAction(nameof(GetById), new { id = nuevoProducto.Id }, nuevoProducto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductoDTO producto)
        {
            if (id != producto.Id)
                return BadRequest();

            await _productoService.ActualizarAsync(producto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.EliminarAsync(id);
            return NoContent();
        }

    }
}
