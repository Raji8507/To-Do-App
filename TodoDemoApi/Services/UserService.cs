using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoDemoApi.Data;
using TodoDemoApi.DTOs;
using TodoDemoApi.Models;

namespace TodoDemoApi.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<AuthResponseDto?> AuthenticateAsync(string username, string password)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;

            var token = GenerateJwtToken(user);
            return new AuthResponseDto(token, user.Id, user.Username, user.Role);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = _configuration.GetSection("JwtSettings");
            var secret = jwtSection.GetValue<string>("Secret") ?? throw new Exception("JWT Secret not set");
            var issuer = jwtSection.GetValue<string>("Issuer") ?? "todo-demo";
            var audience = jwtSection.GetValue<string>("Audience") ?? "todo-demo-audience";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetByIdAsync(int id) => await _db.Users.FindAsync(id);
        public async Task<User?> GetByUsernameAsync(string username) => await _db.Users.SingleOrDefaultAsync(u => u.Username == username);
    }
}
