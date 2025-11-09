namespace TPVY.API.Ecommerce.Models.Auth
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public ICollection<UsuariosAuth>? Usuarios { get; set; }
    }
}
