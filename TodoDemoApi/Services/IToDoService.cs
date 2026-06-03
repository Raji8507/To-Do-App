using TodoDemoApi.DTOs;
using TodoDemoApi.Models;

namespace TodoDemoApi.Services
{
    public interface IToDoService
    {
        Task<ToDo> CreateAsync(CreateToDoDto dto, int createdByUserId);
        Task<IEnumerable<ToDo>> GetAllAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ToDo>> GetByAssignedUserIdAsync(int userId, int pageNumber, int pageSize);
        Task<ToDo?> GetByIdAsync(int id);
        Task<ToDo?> UpdateAsync(int id, UpdateToDoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
