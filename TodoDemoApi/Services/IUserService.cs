using TodoDemoApi.DTOs;
using TodoDemoApi.Models;

namespace TodoDemoApi.Services
{
    public interface IUserService
    {
        Task<AuthResponseDto?> AuthenticateAsync(string username, string password);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> RegisterAsync(string username, string password, string role);
        Task<IEnumerable<User>> GetAllAsync();
    }
}
