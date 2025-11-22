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
        public async Task<IActionResult> Create([FromForm] RegistrerProductoDTO producto, [FromServices] IWebHostEnvironment env)
        {
            if (producto.Imagenes == null || producto.Imagenes.Count == 0)
                return BadRequest("Debe enviar al menos una imagen.");

            var nuevoProducto = await _productoService.CrearAsync(producto,env);
            return CreatedAtAction(nameof(GetById), new { id = nuevoProducto.IdProducto }, nuevoProducto);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Update([FromForm] UpdateProductoDTO producto, [FromServices] IWebHostEnvironment env)
        {
            await _productoService.ActualizarAsync(producto,env);
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
