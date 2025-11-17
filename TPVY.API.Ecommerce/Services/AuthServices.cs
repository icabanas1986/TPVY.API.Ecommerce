using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPVY.API.Ecommerce.Data.Repository;
using TPVY.API.Ecommerce.Models.Auth;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;

        public AuthServices(IAuthRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<string> RegisterAsync(string nombre, string email, string password, int rolId)
        {
            if (await _repository.ObtieneUsuarioPorCorreo(email))
                throw new Exception("El correo ya está registrado.");

            var usuario = new UsuariosAuth
            {
                Nombre = nombre,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RolId = rolId
            };

            await _repository.RegistraUsuario(usuario);
            return "Usuario registrado correctamente.";
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var usuario = await _repository.GetByEmailAsync(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                throw new Exception("Credenciales inválidas.");

            return GenerarToken(usuario);
        }

        public async Task<bool> EliminaUsuario(int id)
        {
            return await _repository.EliminaUsuario(id);
        }
        
        public async Task<bool> ActualizaUsuario(int Id, string Nombre, string Email, int RolId, string Password)
        {
            UsuariosAuth usuarios  = new UsuariosAuth()
            {
                Id = Id,
                Nombre = Nombre,
                Email = Email,
                RolId = RolId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password)
            };
            return await _repository.ActualizaUsuario(usuarios);
        }

        public async Task<List<UsuariosAuth>> ObtenerUsuarios()
        {
            return await _repository.ObtenerUsuarios();
        }
        private string GenerarToken(UsuariosAuth usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim("id", usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "SinRol")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
