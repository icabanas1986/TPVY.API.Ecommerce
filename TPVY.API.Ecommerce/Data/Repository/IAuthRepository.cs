using TPVY.API.Ecommerce.Models.Auth;

namespace TPVY.API.Ecommerce.Data.Repository
{
    public interface IAuthRepository
    {
        Task<int> RegistraUsuario(UsuariosAuth auth);
        Task<UsuariosAuth> ObtieneUsuarioPorId(int id);
        Task<bool> ObtieneUsuarioPorCorreo(string correo);
        Task<UsuariosAuth?> GetByEmailAsync(string email);
    }
}
