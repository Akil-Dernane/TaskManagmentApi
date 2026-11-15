namespace API.Contracts.Tasks
{
    public record CreateTaskItemRequest(
        Guid ProjectId,
        string Title,
        string? Description,
        DateOnly? DueDate);
}
