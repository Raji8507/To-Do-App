using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoDemoApi.Data;
using TodoDemoApi.DTOs;
using TodoDemoApi.Models;

namespace TodoDemoApi.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public ToDoService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ToDo> CreateAsync(CreateToDoDto dto, int createdByUserId)
        {
            var todo = _mapper.Map<ToDo>(dto);
            todo.CreatedByUserId = createdByUserId;
            _db.ToDos.Add(todo);
            await _db.SaveChangesAsync();
            return todo;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _db.ToDos.FindAsync(id);
            if (todo == null)
                return false;

            _db.ToDos.Remove(todo);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ToDo>>
        GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            pageSize = pageSize <= 0 ? 5 : pageSize;

            return await _db.ToDos

                .Include(t => t.AssignedToUser)

                .Include(t => t.CreatedByUser)

                .Skip((pageNumber - 1) * pageSize)

                .Take(pageSize)

                .ToListAsync();
        }

        public async Task<ToDo?>
        GetByIdAsync(int id)
        {
            return await _db.ToDos

                .Include(t => t.AssignedToUser)

                .Include(t => t.CreatedByUser)

                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ToDo>>
        GetByAssignedUserIdAsync(int userId, int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            pageSize = pageSize <= 0 ? 5 : pageSize;

            return await _db.ToDos

                .Where(t =>t.AssignedToUserId == userId)

                .Include(t => t.AssignedToUser)

                .Include(t => t.CreatedByUser)

                .Skip((pageNumber - 1) * pageSize)

                .Take(pageSize)

                .ToListAsync();
        }

        public async Task<ToDo?> UpdateAsync(int id, UpdateToDoDto dto)
        {
            var todo = await _db.ToDos.FindAsync(id);

            if (todo == null)
                return null;

            _mapper.Map(dto, todo);

            await _db.SaveChangesAsync();

            return todo;
        }
    }
}