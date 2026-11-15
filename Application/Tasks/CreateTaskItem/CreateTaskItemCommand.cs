using Domain.Enums;

namespace Application.Tasks.CreateTaskItem
{
    public record CreateTaskItemCommand(
        Guid ProjectId,
        string Title,
        string? Description,
        DateOnly? DueDate);
}
