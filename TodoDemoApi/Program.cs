using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoDemoApi.Data;
using TodoDemoApi.Services;

namespace TodoDemoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Connection
            var connectionString =
                builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IToDoService, ToDoService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors("AllowReactApp");

            app.MapControllers();

            // Apply migrations automatically
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.Run();
        }
    }
}