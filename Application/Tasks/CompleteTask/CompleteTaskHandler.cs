using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;

namespace Application.Tasks.CompleteTask
{
    public class CompleteTaskHandler(
        ITaskItemRepository taskItemRepository,
        ITaskAuthorizationService taskAuthorizationService,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        public async Task Handle(CompleteTaskCommand command, CancellationToken ct)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(command.Id, ct)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!await taskAuthorizationService.CanUpdateTaskAsync(
                taskItem.ProjectId,
                taskItem.AssignedMemberId,
                ct))
                throw new ForbiddenAccessException();

            taskItem.Complete(clock.UtcNow);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}