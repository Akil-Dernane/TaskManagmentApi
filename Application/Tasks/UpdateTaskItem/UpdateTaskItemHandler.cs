using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Application.Tasks.UpdateTaskItem
{
    public class UpdateTaskItemHandler(
        ITaskItemRepository _taskItemRepository,
        IUnitOfWork _unitOfWork,
        IClock _clock)
    {
        public async Task Handle(UpdateTaskItemCommand command, CancellationToken ct)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Task not found");

            var now = _clock.UtcNow;

            taskItem.UpdateDetails(command.Title, command.Description, command.DueDate, now);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
