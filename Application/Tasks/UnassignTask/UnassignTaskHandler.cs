using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Application.Tasks.UnassignTask
{
    public class UnassignTaskHandler(
         ITaskItemRepository _taskItemRepository,
        IClock _clock,
        IUnitOfWork _unitOfWork)
    {
        public async Task Handle(UnassignTaskCommand command, CancellationToken ct)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(command.TaskId, ct) ?? throw new KeyNotFoundException("Task not Found");

            taskItem.Unassign(_clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
