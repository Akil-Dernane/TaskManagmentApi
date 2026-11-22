using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Tasks.UnassignTask
{
    public class UnassignTaskHandler(
        ITaskItemRepository taskItemRepository,
        ITaskAuthorizationService taskAuthorizationService,
        IClock clock,
        IUnitOfWork unitOfWork)
    {
        public async Task Handle(UnassignTaskCommand command, CancellationToken ct)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(command.TaskId, ct)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!await taskAuthorizationService.HasPermissionAsync(
                taskItem.ProjectId,
                TaskPermission.AssignTasks,
                ct))
                throw new ForbiddenAccessException();

            taskItem.Unassign(clock.UtcNow);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}