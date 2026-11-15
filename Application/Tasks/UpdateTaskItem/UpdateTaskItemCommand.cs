namespace Application.Tasks.UpdateTaskItem
{
    public record UpdateTaskItemCommand(
        Guid Id,
        string Title,
        string? Description,
        DateOnly? DueDate);
}
