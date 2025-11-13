using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Models.Auth;

namespace TPVY.API.Ecommerce.Repositories.Interfaces
{
    public interface IRolRepository
    {
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
        Task<Rol> ObtenerPorIdAsync(int id);
        Task<Rol> CrearAsync(Rol producto);
        Task ActualizarAsync(Rol producto);
        Task EliminarAsync(int id);
    }
}
