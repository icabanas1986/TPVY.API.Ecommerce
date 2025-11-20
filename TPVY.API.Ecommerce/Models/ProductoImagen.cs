namespace TPVY.API.Ecommerce.Models
{
    public class ProductoImagen
    {
        public int Id { get; set; }

        public string Url { get; set; } = string.Empty;

        // Relación
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}
