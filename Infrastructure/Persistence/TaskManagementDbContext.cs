using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence
{
    public class TaskManagementDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<ProjectMembership> ProjectMemberships { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TaskManagementDbContext).Assembly);
        }
    }
}
