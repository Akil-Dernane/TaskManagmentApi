using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Application.Tasks.StartTask
{
    public class StartTaskHandler(
        ITaskItemRepository _taskItemRepository,
        IUnitOfWork _unitOfWork,
        IClock _clock)
    {
        public async Task Handle(StartTaskCommand command, CancellationToken ct) 
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("TAsk not found");

            taskItem.Start(_clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
