using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Application.Tasks.CompleteTask
{
    public class CompleteTaskHandler(
        ITaskItemRepository _taskItemRepository,
        IUnitOfWork _unitOfWork,
        IClock _clock)
    {
        public async Task Handle(CompleteTaskCommand command, CancellationToken ct)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Task not found.");

            taskItem.Complete(_clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
