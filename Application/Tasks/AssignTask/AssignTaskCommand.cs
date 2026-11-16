namespace Application.Tasks.AssignTask
{
    public record AssignTaskCommand(
        Guid TaskId,
        Guid MemberId);
}
