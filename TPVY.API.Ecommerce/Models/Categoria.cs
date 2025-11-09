namespace TPVY.API.Ecommerce.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        // Relación 1 a N
        public ICollection<Producto>? Productos { get; set; } 
    }
}
