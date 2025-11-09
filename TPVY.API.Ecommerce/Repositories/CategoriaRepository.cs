using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;
using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Repositories.Interfaces;

namespace TPVY.API.Ecommerce.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoriaRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Categoria>> GetAllAsync() =>
            await _context.Categorias.Include(c => c.Productos).ToListAsync();

        public async Task<Categoria?> GetByIdAsync(int id) =>
            await _context.Categorias.Include(c => c.Productos)
                                     .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Categoria categoria) => await _context.Categorias.AddAsync(categoria);
        public async Task UpdateAsync(Categoria categoria) => _context.Categorias.Update(categoria);
        public async Task DeleteAsync(int id)
        {
            var cat = await _context.Categorias.FindAsync(id);
            if (cat != null) _context.Categorias.Remove(cat);
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
