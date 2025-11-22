using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TPVY.API.Ecommerce.DTOs.ImagenesProducto;
using TPVY.API.Ecommerce.DTOs.Producto;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Repositories.Interfaces;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Services
{
    public class ProductoService:IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IProductoImagenesRepository _productoImagenesRepository;
        private readonly ILogger<ProductoService> _logger;
        public ProductoService(IProductoRepository productoRepository,IProductoImagenesRepository productoImagenesRepository, ILogger<ProductoService> logger)
        {
            _productoRepository = productoRepository;
            _productoImagenesRepository = productoImagenesRepository;
            _logger = logger;
        }

        public async Task<List<ObtieneProductoDTO>> ObtenerTodosAsync()
        {
            var productos = await _productoRepository.ObtenerTodosAsync();
            List<ObtieneProductoDTO> productosDTO = new List<ObtieneProductoDTO>();
            foreach (var product in productos)
            {
                var imagenes = await _productoImagenesRepository.ObtenerImagenesPorProductoIdAsync(product.Id);
                productosDTO.Add(new ObtieneProductoDTO()
                {
                    IdProducto = product.Id,
                    Nombre = product.Nombre,
                    Descripcion = product.Descripcion,
                    Precio = product.Precio,
                    Stock = product.Stock,
                    IdCategoria = product.CategoriaId,
                    Imagenes = imagenes.Select(img => new ObtenImagenesProductoDTO
                    {
                        IdImagen = img.Id,
                        Url = img.Url
                    }).ToList()
                });
            }
            return productosDTO;
        }            
        

        public async Task<ObtieneProductoDTO> ObtenerPorIdAsync(int id)
        {
            var productos =  await _productoRepository.ObtenerPorIdAsync(id);
            if(productos == null)
            {
                return null;
            }
            var imagenes = await _productoImagenesRepository.ObtenerImagenesPorProductoIdAsync(productos.Id);
            ObtieneProductoDTO productoDTO = new ObtieneProductoDTO()
            {
                IdProducto = productos.Id,
                Nombre = productos.Nombre,
                Descripcion = productos.Descripcion,
                Precio = productos.Precio,
                Stock = productos.Stock,
                IdCategoria = productos.CategoriaId,
                Imagenes = imagenes.Select(img => new ObtenImagenesProductoDTO
                {
                    IdImagen = img.Id,
                    Url = img.Url
                }).ToList()
            };
            return productoDTO;
        }

        public async Task<ObtieneProductoDTO> CrearAsync(RegistrerProductoDTO productoDTO,[FromServices] IWebHostEnvironment env)
        {

            List<ProductoImagen> productoImagen = new List<ProductoImagen>();

            var urls = this.GuardaImagenes(productoDTO.Imagenes, env);            

            Producto producto = new Producto()
            {
                Activo = productoDTO.Activo,
                CategoriaId = productoDTO.CategoriaID,
                Descripcion = productoDTO.Descripcion,
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
            };

            var product = await _productoRepository.CrearAsync(producto);
            if(product == null)
            {
                return null;
            }

            foreach (var url in await urls)
            {
                productoImagen.Add(new ProductoImagen()
                {
                    ProductoId = product.Id,
                    Url = url
                });
            }
            await _productoImagenesRepository.GuardarImagenesAsync(productoImagen);

            return await this.ObtenerPorIdAsync(product.Id);
        }

        public async Task ActualizarAsync(UpdateProductoDTO productoDTO, [FromServices] IWebHostEnvironment env)
        {
            List<ProductoImagen> productoImagen = new List<ProductoImagen>();
            //Primero eliminamos las imagenes para despues insertar las nuevas si es que cambiaron
            var eliminaImagenes = await _productoImagenesRepository.EliminaImagenesPorIdProductoAsync(productoDTO.Id);
            if(!eliminaImagenes)
            {
                _logger.LogError("No se pudieron eliminar las imagenes del producto con Id: {ProductoId}", productoDTO.Id);
                throw new Exception("No se puede actualizar las imagenes");
            }
            var urls = this.GuardaImagenes(productoDTO.Imagenes, env);
            _logger.LogInformation("Actualizando el producto con Id: {ProductoId}", productoDTO.Id);

            Producto producto = new Producto()
            {
                Activo = productoDTO.Activo,
                CategoriaId = productoDTO.CategoriaID,
                Descripcion = productoDTO.Descripcion,
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                Id = productoDTO.Id
            };
            await _productoRepository.ActualizarAsync(producto);

            foreach (var url in await urls)
            {
                productoImagen.Add(new ProductoImagen()
                {
                    ProductoId = productoDTO.Id,
                    Url = url
                });
            }
            await _productoImagenesRepository.GuardarImagenesAsync(productoImagen);
        }

        public async Task EliminarAsync(int id)
        {
            //Se eliminan los producto imagenes asociados
            var borrarImagenes = await _productoImagenesRepository.EliminaImagenesPorIdProductoAsync(id);
            if(!borrarImagenes)
            {
                throw new Exception("No se pudieron eliminar las imagenes asociadas al producto.");
            }
            //Se elimina el producto
            await _productoRepository.EliminarAsync(id);
        }

        public async Task<List<string>> GuardaImagenes(List<IFormFile> archivos, [FromServices] IWebHostEnvironment env)
        {
            List<string> imagenUrls = new List<string>();
            try
            {
                var rutaBase = env.ContentRootPath;
                string uploadsFolder = Path.Combine(rutaBase, "imagenes_productos");
                _logger.LogInformation("Guardando imagenes en la ruta: {UploadsFolder}", uploadsFolder);
                if (!Directory.Exists(uploadsFolder))
                {
                    _logger.LogInformation("La carpeta no existe. Creando carpeta en la ruta: {UploadsFolder}", uploadsFolder);
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in archivos)
                {
                    _logger.LogInformation("Guardando imagen: {FileName}", file.FileName);
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    _logger.LogInformation("Nombre del archivo generado: {FileName}", fileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    _logger.LogInformation("Ruta completa del archivo: {FilePath}", filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    _logger.LogInformation("Imagen guardada exitosamente: {FilePath}", filePath);
                    imagenUrls.Add(filePath);
                }
                return imagenUrls;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar las imagenes.");
                throw;
            }
            
        }
    }
}
