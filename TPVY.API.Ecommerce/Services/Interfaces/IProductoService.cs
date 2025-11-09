using TPVY.API.Ecommerce.DTOs;
using TPVY.API.Ecommerce.Models;

namespace TPVY.API.Ecommerce.Services.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto> ObtenerPorIdAsync(int id);
        Task<Producto> CrearAsync(RegistrerProductoDTO producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
    }
}
