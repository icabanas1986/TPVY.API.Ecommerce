using System.ComponentModel.DataAnnotations.Schema;

namespace TPVY.API.Ecommerce.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public int Stock { get; set; } = 0;
        public bool Activo { get; set; } = true;
        public string? ImagenUrl { get; set; }

        // Relación con Categoría
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public List<ProductoImagen> Imagenes { get; set; } = new();
    }
}
