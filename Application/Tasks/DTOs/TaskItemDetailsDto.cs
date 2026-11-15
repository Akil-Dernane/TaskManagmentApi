
using Domain.Enums;

namespace Application.Tasks.DTOs
{
    public record TaskItemDetailsDto(
        Guid Id,
        Guid ProjectId,
        string Title,
        string? Description,
        ProjectTaskStatus Status,
        ProjectTaskPriority Priority,
        DateOnly? DueDate,
        DateTime? CompletedAt,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
