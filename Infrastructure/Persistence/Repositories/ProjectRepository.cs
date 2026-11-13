using Application.Abstractions.Persistence;
using Application.Projects.GetProjectById;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly TaskManagementDbContext _dbContext;
        public ProjectRepository(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async System.Threading.Tasks.Task AddAsync(Project project, CancellationToken ct)
        {
            await _dbContext.Projects.AddAsync(project, ct);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
        {
            var normalizedName = name.ToLower();

            return await _dbContext.Projects
                .AnyAsync(p => p.Name.ToLower() == normalizedName, ct);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<ProjectDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.Projects
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProjectDetailsDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CreatedAtUtc,
                    p.Status))
                .FirstOrDefaultAsync(ct);
        }
    }
}
