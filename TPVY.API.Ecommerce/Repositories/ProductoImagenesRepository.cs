using TPVY.API.Ecommerce.Data;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Repositories.Interfaces;

namespace TPVY.API.Ecommerce.Repositories
{
    public class ProductoImagenesRepository: IProductoImagenesRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoImagenesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<string>> GuardarImagenesAsync(List<ProductoImagen> imagenes)
        {
            IEnumerable<string> urls;
            try
            {
                _context.ProductoImagenes.AddRange(imagenes);
                var result = _context.SaveChangesAsync();
                urls = imagenes.Select(img => img.Url);
            }
            catch (Exception ex)
            {
                return Task.FromException<IEnumerable<string>>(ex);
            }
            return Task.FromResult(urls);
        }

        public Task<IEnumerable<ProductoImagen>> ObtenerImagenesPorProductoIdAsync(int productoId)
        {
            var imagenes = _context.ProductoImagenes.Where(img => img.ProductoId == productoId).AsEnumerable();
            return Task.FromResult(imagenes);
        }

        public Task<bool> EliminaImagenesPorIdProductoAsync(int productoId)
        {
            try
            {
                var imagenes = _context.ProductoImagenes.Where(img => img.ProductoId == productoId);
                _context.ProductoImagenes.RemoveRange(imagenes);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromException<bool>(ex);
            }
        }
    }
}
