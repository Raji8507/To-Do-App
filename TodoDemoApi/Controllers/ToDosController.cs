using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoDemoApi.DTOs;
using TodoDemoApi.Services;

namespace TodoDemoApi.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]

    [Route(
    "api/v{version:apiVersion}/[controller]")]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _todoService;
        private readonly IMapper _mapper;
        public ToDosController(IToDoService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)?? "0");

        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role)?? "";

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 5)
        {
            if (CurrentUserRole == "Manager")
            {
                var todos = await _todoService.GetAllAsync(pageNumber, pageSize);
                var dtoList = _mapper.Map<IEnumerable<ToDoDTO>>(todos);
                return Ok(dtoList);
            }

            var myTodos = await _todoService.GetByAssignedUserIdAsync(CurrentUserId, pageNumber, pageSize);

            var myDtoList = _mapper.Map<IEnumerable<ToDoDTO>>(myTodos);
            return Ok(myDtoList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult>
        Get(int id)
        {
            var todo = await _todoService.GetByIdAsync(id);

            if (todo == null)
                return NotFound();
            if (CurrentUserRole == "Manager" || todo.AssignedToUserId == CurrentUserId)
            {
                return Ok(_mapper.Map<ToDoDTO>(todo));
            }
            return Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult>
        Create(CreateToDoDto dto)
        {
            var todo = await _todoService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, _mapper.Map<ToDoDTO>(todo));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateToDoDto dto)
        {
            var todo = await _todoService.GetByIdAsync(id);
            if (todo == null)
                return NotFound();
            if (CurrentUserRole == "Manager" || todo.AssignedToUserId == CurrentUserId)
            {
                var updated = await _todoService.UpdateAsync(id, dto);

                return Ok(_mapper.Map<ToDoDTO>(updated));
            }
            return Forbid();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _todoService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}