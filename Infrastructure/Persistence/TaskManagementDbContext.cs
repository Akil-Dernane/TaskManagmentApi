using Microsoft.EntityFrameworkCore;
using Domain.Entities;
namespace Infrastructure.Persistence
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TaskManagementDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
