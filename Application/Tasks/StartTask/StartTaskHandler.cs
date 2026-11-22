using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;

namespace Application.Tasks.StartTask
{
    public class StartTaskHandler(
        ITaskItemRepository taskItemRepository,
        ITaskAuthorizationService taskAuthorizationService,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        public async Task Handle(StartTaskCommand command, CancellationToken ct)
        {
            var taskItem = await taskItemRepository.GetByIdAsync(command.Id, ct)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!await taskAuthorizationService.CanUpdateTaskAsync(
                taskItem.ProjectId,
                taskItem.AssignedMemberId,
                ct))
                throw new ForbiddenAccessException();

            taskItem.Start(clock.UtcNow);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}