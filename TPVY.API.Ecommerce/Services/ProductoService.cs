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

        public ProductoService(IProductoRepository productoRepository,IProductoImagenesRepository productoImagenesRepository)
        {
            _productoRepository = productoRepository;
            _productoImagenesRepository = productoImagenesRepository;
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
                throw new Exception("No se puede actualizar las imagenes");
            }
            var urls = this.GuardaImagenes(productoDTO.Imagenes, env);

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
            var rutaBase = env.ContentRootPath;
            string uploadsFolder = Path.Combine(rutaBase, "imagenes_productos");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var file in archivos)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Guardar la URL relativa en la BD
                //imagenUrls.Add($"/imagenes_productos/{fileName}");
                imagenUrls.Add(filePath);
            }
            return imagenUrls;
        }
    }
}
