using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Application.Tasks.ChangeTaskPriority
{
    public class ChangeTaskPriorityHandler(
        ITaskItemRepository _taskItemRepository,
        IUnitOfWork _unitOfWork,
        IClock _clock)
    {
        public async Task Handle(ChangeTaskPriorityCommand command, CancellationToken ct)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Task not found");

            taskItem.ChangePriority(command.Priority, _clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
