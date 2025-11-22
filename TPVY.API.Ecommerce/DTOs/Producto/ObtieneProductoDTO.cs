using TPVY.API.Ecommerce.DTOs.ImagenesProducto;

namespace TPVY.API.Ecommerce.DTOs.Producto
{
    public class ObtieneProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public List<ObtenImagenesProductoDTO> Imagenes { get; set; }
    }
}
