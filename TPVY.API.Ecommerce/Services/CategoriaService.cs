using TPVY.API.Ecommerce.Models;
using TPVY.API.Ecommerce.Repositories.Interfaces;
using TPVY.API.Ecommerce.Services.Interfaces;

namespace TPVY.API.Ecommerce.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Categoria>> ObtenerCategorias() => await _repository.GetAllAsync();
        public async Task<Categoria?> ObtenerPorId(int id) => await _repository.GetByIdAsync(id);

        public async Task CrearCategoria(Categoria categoria)
        {
            await _repository.AddAsync(categoria);
            await _repository.SaveChangesAsync();
        }

        public async Task ActualizarCategoria(Categoria categoria)
        {
            await _repository.UpdateAsync(categoria);
            await _repository.SaveChangesAsync();
        }

        public async Task EliminarCategoria(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }
}
