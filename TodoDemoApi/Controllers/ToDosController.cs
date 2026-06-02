using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;

        public ToDosController(
            IToDoService todoService,
            IUserService userService,
            IMapper mapper)
        {
            _todoService = todoService;
            _userService = userService;
            _mapper = mapper;
        }

        private int CurrentUserId =>
            int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(ClaimTypes.Name)
                ?? "0");

        private string CurrentUserRole =>
            User.FindFirstValue(ClaimTypes.Role) ?? "";

        // GET api/todos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (CurrentUserRole == "Manager")
            {
                var todos = await _todoService.GetAllAsync();

                var dtoList =
                    _mapper.Map<IEnumerable<ToDoDTO>>(todos);

                return Ok(dtoList);
            }

            var myTodos =
                await _todoService
                .GetByAssignedUserIdAsync(CurrentUserId);

            var myDtoList =
                _mapper.Map<IEnumerable<ToDoDTO>>(myTodos);

            return Ok(myDtoList);
        }

        // GET api/todos/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
                return NotFound();

            if (CurrentUserRole == "Manager"
                || todo.AssignedToUserId == CurrentUserId)
            {
                var dto =
                    _mapper.Map<ToDoDTO>(todo);

                return Ok(dto);
            }

            return Forbid();
        }

        // POST api/todos
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(
            CreateToDoDto dto)
        {
            var todo =
                await _todoService
                .CreateAsync(dto, CurrentUserId);

            var response =
                _mapper.Map<ToDoDTO>(todo);

            return CreatedAtAction(
                nameof(Get),
                new { id = todo.Id },
                response);
        }

        // PUT api/todos/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateToDoDto dto)
        {
            var todo =
                await _todoService.GetByIdAsync(id);

            if (todo == null)
                return NotFound();

            if (CurrentUserRole == "Manager"
                || todo.AssignedToUserId == CurrentUserId)
            {
                var updated =
                    await _todoService
                    .UpdateAsync(id, dto);

                return Ok(
                    _mapper.Map<ToDoDTO>(updated));
            }

            return Forbid();
        }

        // DELETE api/todos/1
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted =
                await _todoService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}