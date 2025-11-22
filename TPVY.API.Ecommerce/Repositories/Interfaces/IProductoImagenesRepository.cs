using TPVY.API.Ecommerce.Models;

namespace TPVY.API.Ecommerce.Repositories.Interfaces
{
    public interface IProductoImagenesRepository
    {
        Task<IEnumerable<string>> GuardarImagenesAsync(List<ProductoImagen> imagenes);
        Task<IEnumerable<ProductoImagen>> ObtenerImagenesPorProductoIdAsync(int productoId);
        Task<bool> EliminaImagenesPorIdProductoAsync(int productoId);
    }
}
