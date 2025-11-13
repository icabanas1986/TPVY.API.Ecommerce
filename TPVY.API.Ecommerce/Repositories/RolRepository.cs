using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TPVY.API.Ecommerce.Data;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Models.Auth;
using TPVY.API.Ecommerce.Repositories.Interfaces;

namespace TPVY.API.Ecommerce.Repositories
{
    public class RolRepository: IRolRepository
    {
        private readonly ApplicationDbContext _context;

        public RolRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async  Task<Rol> ObtenerPorIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Rol> CrearAsync(Rol rol)
        {
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task ActualizarAsync(Rol rol)
        {
            _context.Entry(rol).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
            }
        }
    }
}
