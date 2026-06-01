using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using TodoDemoApi.DTOs;
using TodoDemoApi.Services;

namespace TodoDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _todoService;
        private readonly IUserService _userService;

        public ToDosController(IToDoService todoService, IUserService userService)
        {
            _todoService = todoService;
            _userService = userService;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? "0");

        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? "";

        // Manager: get all; Employee: get only assigned
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (CurrentUserRole == "Manager")
            {
                var all = await _todoService.GetAllAsync(); // returns IEnumerable<ToDo>
                var dtoList = all.Select(t => new ToDoDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.DueDate,
                    t.AssignedToUserId,
                    t.CreatedByUserId,
                    t.AssignedToUser?.Username,
                    t.CreatedByUser?.Username
                ));
                return Ok(dtoList);
            }
            else
            {
                var mine = await _todoService.GetByAssignedUserIdAsync(CurrentUserId);
                var dtoList = mine.Select(t => new ToDoDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.DueDate,
                    t.AssignedToUserId,
                    t.CreatedByUserId,
                    t.AssignedToUser?.Username,
                    t.CreatedByUser?.Username
                ));
                return Ok(dtoList);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var t = await _todoService.GetByIdAsync(id);
            if (t == null) return NotFound();

            if (CurrentUserRole == "Manager" || t.AssignedToUserId == CurrentUserId)
            {
                var dto = new ToDoDTO(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.IsCompleted,
                    t.DueDate,
                    t.AssignedToUserId,
                    t.CreatedByUserId,
                    t.AssignedToUser?.Username,
                    t.CreatedByUser?.Username
                );
                return Ok(dto);
            }

            return Forbid();
        }

        // Only Manager can create and assign tasks
        [HttpPost]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> Create(CreateToDoDto dto)
        {
            var todo = await _todoService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
        }

        // Managers can update any; employees can update only their assigned tasks (e.g. mark complete)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateToDoDto dto)
        {
            var t = await _todoService.GetByIdAsync(id);
            if (t == null) return NotFound();

            if (CurrentUserRole == "Manager" || t.AssignedToUserId == CurrentUserId)
            {
                var updated = await _todoService.UpdateAsync(id, dto);
                return Ok(updated);
            }

            return Forbid();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _todoService.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
