namespace TPVY.API.Ecommerce.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Rol { get; set; } = "Cliente"; // Admin o Cliente
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
