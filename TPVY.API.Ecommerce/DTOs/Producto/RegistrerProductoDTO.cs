namespace TPVY.API.Ecommerce.DTOs.Producto
{
    public class RegistrerProductoDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public string imagenUrl { get; set; }
        public int CategoriaID { get; set; }
    }
}
