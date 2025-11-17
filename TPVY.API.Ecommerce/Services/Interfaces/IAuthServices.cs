using TPVY.API.Ecommerce.Models.Auth;

namespace TPVY.API.Ecommerce.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(string nombre, string email, string password, int rolId);
        Task<string> LoginAsync(string email, string password);

        Task<bool> EliminaUsuario(int id);

        Task<bool> ActualizaUsuario(int Id, string Nombre, string Email, int RolId, string Password);

        Task<List<UsuariosAuth>> ObtenerUsuarios();
    }
}
