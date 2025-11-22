using Microsoft.AspNetCore.Mvc;
using TPVY.API.Ecommerce.DTOs.Producto;
using TPVY.API.Ecommerce.Models;

namespace TPVY.API.Ecommerce.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ObtieneProductoDTO>> ObtenerTodosAsync();
        Task<ObtieneProductoDTO> ObtenerPorIdAsync(int id);
        Task<ObtieneProductoDTO> CrearAsync(RegistrerProductoDTO productoDTO, [FromServices] IWebHostEnvironment env);
        Task ActualizarAsync(UpdateProductoDTO producto, [FromServices] IWebHostEnvironment env);
        Task EliminarAsync(int id);
    }
}
