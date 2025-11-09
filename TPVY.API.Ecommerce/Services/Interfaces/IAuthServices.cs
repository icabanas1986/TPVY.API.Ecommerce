namespace TPVY.API.Ecommerce.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(string nombre, string email, string password, int rolId);
        Task<string> LoginAsync(string email, string password);
    }
}
