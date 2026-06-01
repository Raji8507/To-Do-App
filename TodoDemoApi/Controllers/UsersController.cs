using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoDemoApi.DTOs;
using TodoDemoApi.Models;
using TodoDemoApi.Services;

namespace TodoDemoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private int CurrentUserId
        {
            get
            {
                var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
                return int.TryParse(idClaim, out var id) ? id : 0;
            }
        }

        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;

        // GET: api/users
        // Manager: return all users. Employee: return only their own user info.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (string.Equals(CurrentUserRole, "Manager", StringComparison.OrdinalIgnoreCase))
            {
                var users = await _userService.GetAllAsync();
                var dtos = users.Select(u => new UserDTO(u.Id, u.Username, u.Role));
                return Ok(dtos);
            }
            else
            {
                var user = await _userService.GetByIdAsync(CurrentUserId);
                if (user == null) return NotFound();
                var dto = new UserDTO(user.Id, user.Username, user.Role);
                return Ok(new[] { dto });
            }
        }

        // GET: api/users/me
        // Returns current user info
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userService.GetByIdAsync(CurrentUserId);
            if (user == null) return NotFound();
            return Ok(new UserDTO(user.Id, user.Username, user.Role));
        }

        // Optional: GET api/users/{id}
        // Manager or self can get details
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            if (string.Equals(CurrentUserRole, "Manager", StringComparison.OrdinalIgnoreCase) || user.Id == CurrentUserId)
            {
                return Ok(new UserDTO(user.Id, user.Username, user.Role));
            }
            return Forbid();
        }
    }
}