using TPVY.API.Ecommerce.Models;

namespace TPVY.API.Ecommerce.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> ObtenerCategorias();
        Task<Categoria?> ObtenerPorId(int id);
        Task CrearCategoria(Categoria categoria);
        Task ActualizarCategoria(Categoria categoria);
        Task EliminarCategoria(int id);
    }
}
