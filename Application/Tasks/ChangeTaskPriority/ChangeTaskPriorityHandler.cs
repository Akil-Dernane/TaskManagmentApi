using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Tasks.ChangeTaskPriority
{
    public class ChangeTaskPriorityHandler(
        ITaskItemRepository taskItemRepository,
        ITaskAuthorizationService taskAuthorizationService,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        public async Task Handle(ChangeTaskPriorityCommand command,CancellationToken ct)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(command.Id, ct)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!await taskAuthorizationService.HasPermissionAsync(
                taskItem.ProjectId,
                TaskPermission.ChangeTaskPriority,
                ct))
                throw new ForbiddenAccessException();

            taskItem.ChangePriority(command.Priority, clock.UtcNow);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}