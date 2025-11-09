using TPVY.API.Ecommerce.DTOs;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Repositories.Interfaces;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Services
{
    public class ProductoService:IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _productoRepository.ObtenerTodosAsync();
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            return await _productoRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Producto> CrearAsync(RegistrerProductoDTO productoDTO)
        {
            Producto producto = new Producto()
            {
                Activo = productoDTO.Activo,
                CategoriaId = productoDTO.CategoriaID,
                Descripcion = productoDTO.Descripcion,
                ImagenUrl = productoDTO.imagenUrl,
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock
            };

            return await _productoRepository.CrearAsync(producto);
        }

        public async Task ActualizarAsync(Producto producto)
        {
            await _productoRepository.ActualizarAsync(producto);
        }

        public async Task EliminarAsync(int id)
        {
            await _productoRepository.EliminarAsync(id);
        }
    }
}
