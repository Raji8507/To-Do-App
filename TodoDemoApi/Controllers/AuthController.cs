using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoDemoApi.DTOs;
using TodoDemoApi.Services;

namespace TodoDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _users;

        public AuthController(IUserService users) { _users = users; }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existing = await _users.GetByUsernameAsync(dto.Username);
            if (existing != null) return BadRequest("Username already exists");

            var user = await _users.RegisterAsync(dto.Username, dto.Password, dto.Role);
            return CreatedAtAction(null, new { user.Id, user.Username, user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var auth = await _users.AuthenticateAsync(dto.Username, dto.Password);
            if (auth == null) return Unauthorized("Invalid credentials");
            return Ok(auth);
        }
    }
}
