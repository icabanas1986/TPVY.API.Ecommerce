namespace TPVY.API.Ecommerce.Models.Auth
{
    public class UsuariosAuth
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
