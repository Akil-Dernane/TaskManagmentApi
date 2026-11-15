using Domain.Enums;

namespace Application.Tasks.ChangeTaskPriority
{
    public record ChangeTaskPriorityCommand(Guid Id, ProjectTaskPriority Priority);
}
