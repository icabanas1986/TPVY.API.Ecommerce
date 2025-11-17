using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TPVY.API.Ecommerce.Models.Auth;

namespace TPVY.API.Ecommerce.Data.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<int> RegistraUsuario(UsuariosAuth auth)
        {
            int idUsuario = 0;
            try
            {
                _context.UsuariosAuth.Add(auth);
                int res = await _context.SaveChangesAsync();
                if (res > 0) 
                {
                    idUsuario = auth.Id;
                }
            }
            catch (Exception ex)
            {
                idUsuario = -1;
            }
            return idUsuario;
        }

        public async Task<UsuariosAuth> ObtieneUsuarioPorId(int id)
        {
            UsuariosAuth? user = new UsuariosAuth();
            try
            {
                user = await _context.UsuariosAuth.FindAsync(id);
                if(user == null)
                {
                    return new UsuariosAuth();
                }
            }
            catch (Exception ex)
            {
                user = null;
            }
            return user;
        }

        public async Task<bool> ObtieneUsuarioPorCorreo(string correo)
        {
            var existe = false;
            try
            {
                var user = await _context.UsuariosAuth.FirstOrDefaultAsync(x=>x.Email == correo);
                if (user == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return existe;
        }

        public async Task<UsuariosAuth?> GetByEmailAsync(string email)
        {
            return await _context.UsuariosAuth
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EliminaUsuario(int id)
        {
            bool eliminado = false;
            try
            {
                var user = await _context.UsuariosAuth.FindAsync(id);
                if (user != null)
                {
                    _context.UsuariosAuth.Remove(user);
                    int res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {
                        eliminado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                eliminado = false;
            }
            return eliminado;
        }

        public async Task<bool> ActualizaUsuario(UsuariosAuth auth)
        {
            bool actualizado = false;
            try
            {
                _context.UsuariosAuth.Update(auth);
                int res = await _context.SaveChangesAsync();
                if (res > 0)
                {
                    actualizado = true;
                }
            }
            catch (Exception ex)
            {
                actualizado = false;
            }
            return actualizado;
        }

        public async Task<List<UsuariosAuth>> ObtenerUsuarios()
        {
            return await _context.UsuariosAuth.Include(u => u.Rol).ToListAsync();
        }
    }
}
