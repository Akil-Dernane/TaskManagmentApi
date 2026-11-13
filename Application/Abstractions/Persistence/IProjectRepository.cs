using Application.Projects.GetProjectById;
using Domain.Entities;

namespace Application.Abstractions.Persistence;

public interface IProjectRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task AddAsync(Project project, CancellationToken ct);

    Task<ProjectDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct);

    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct);
}