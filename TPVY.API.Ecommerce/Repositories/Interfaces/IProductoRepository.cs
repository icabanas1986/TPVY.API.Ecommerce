using TPVY.API.Ecommerce.Models;

namespace TPVY.API.Ecommerce.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto> ObtenerPorIdAsync(int id);
        Task<Producto> CrearAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
    }
}
