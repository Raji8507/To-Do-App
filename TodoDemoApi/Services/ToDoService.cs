using Microsoft.EntityFrameworkCore;
using TodoDemoApi.Data;
using TodoDemoApi.DTOs;
using TodoDemoApi.Models;

namespace TodoDemoApi.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _db;
        public ToDoService(AppDbContext db) { _db = db; }

        public async Task<ToDo> CreateAsync(CreateToDoDto dto, int createdByUserId)
        {
            var todo = new ToDo
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedByUserId = createdByUserId
            };
            _db.ToDos.Add(todo);
            await _db.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var t = await _db.ToDos.FindAsync(id);
            if (t == null) return false;
            _db.ToDos.Remove(t);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ToDo>> GetAllAsync()
        {
            return await _db.ToDos
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }

        public async Task<ToDo?> GetByIdAsync(int id)
        {
            return await _db.ToDos
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ToDo>> GetByAssignedUserIdAsync(int userId)
        {
            return await _db.ToDos
                .Where(t => t.AssignedToUserId == userId)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .ToListAsync();
        }

        public async Task<ToDo?> UpdateAsync(int id, UpdateToDoDto dto)
        {
            var t = await _db.ToDos.FindAsync(id);
            if (t == null) return null;

            if (dto.Title != null) t.Title = dto.Title;
            if (dto.Description != null) t.Description = dto.Description;
            if (dto.DueDate.HasValue) t.DueDate = dto.DueDate;
            if (dto.IsCompleted.HasValue) t.IsCompleted = dto.IsCompleted.Value;
            if (dto.AssignedToUserId.HasValue) t.AssignedToUserId = dto.AssignedToUserId;

            await _db.SaveChangesAsync();
            return t;
        }
    }
}
