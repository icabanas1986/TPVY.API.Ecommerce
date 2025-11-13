using TPVY.API.Ecommerce.DTOs.Producto;
using TPVY.API.Ecommerce.DTOs.Rol;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Models.Auth;
using TPVY.API.Ecommerce.Repositories;
using TPVY.API.Ecommerce.Repositories.Interfaces;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Services
{
    public class RolService:IRolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            return await _repository.ObtenerTodosAsync();
        }

        public async Task<Rol> ObtenerPorIdAsync(int id)
        {
            return await _repository.ObtenerPorIdAsync(id);
        }

        public async Task<Rol> CrearAsync(RegistrerRolDTO rolDTO)
        {
            Rol rol = new Rol()
            {
                Nombre = rolDTO.Nombre
            };

            return await _repository.CrearAsync(rol);
        }

        public async Task ActualizarAsync(UpdateRolDTO rolDTO)
        {
            Rol rol = new Rol()
            {
                Id = rolDTO.IdRol,
                Nombre = rolDTO.Nombre
            };
            await _repository.ActualizarAsync(rol);
        }

        public async Task EliminarAsync(int id)
        {
            await _repository.EliminarAsync(id);
        }
    }
}
