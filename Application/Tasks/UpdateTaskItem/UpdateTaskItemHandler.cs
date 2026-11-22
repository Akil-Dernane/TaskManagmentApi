using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;

namespace Application.Tasks.UpdateTaskItem
{
    public class UpdateTaskItemHandler(
        ITaskItemRepository taskItemRepository,
        ITaskAuthorizationService taskAuthorizationService,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        public async Task Handle(UpdateTaskItemCommand command, CancellationToken ct)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(command.Id, ct)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!await taskAuthorizationService.CanUpdateTaskAsync(
                taskItem.ProjectId,
                taskItem.AssignedMemberId,
                ct))
                throw new ForbiddenAccessException();

            var now = clock.UtcNow;

            taskItem.UpdateDetails(
                command.Title,
                command.Description,
                command.DueDate,
                now);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}