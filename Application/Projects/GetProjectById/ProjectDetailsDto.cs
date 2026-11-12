using Domain.Enums;

namespace Application.Projects.GetProjectById;

public record ProjectDetailsDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAtUtc,
    ProjectStatus Status
);