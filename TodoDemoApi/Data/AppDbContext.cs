using Microsoft.EntityFrameworkCore;
using TodoDemoApi.Models;

namespace TodoDemoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ToDo> ToDos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedToDos)
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedToDos)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Seed data
            var managerHash = BCrypt.Net.BCrypt.HashPassword("Password123!");
            var employeeHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "manager1",
                    PasswordHash = managerHash,
                    Role = "Manager"
                },
                new User
                {
                    Id = 2,
                    Username = "employee1",
                    PasswordHash = employeeHash,
                    Role = "Employee"
                }
            );
        }
    }
}
