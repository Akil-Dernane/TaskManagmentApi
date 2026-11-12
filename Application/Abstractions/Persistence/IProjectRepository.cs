using Application.Projects.GetProjectById;
using Domain.Entities;

namespace Application.Abstractions.Persistence;

public interface IProjectRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    System.Threading.Tasks.Task AddAsync(Project project, CancellationToken ct);

    Task<ProjectDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct);
}