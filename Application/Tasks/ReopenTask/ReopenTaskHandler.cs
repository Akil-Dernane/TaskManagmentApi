using Application.Abstractions;
using Application.Abstractions.Persistence;
using Domain.Entities;

namespace Application.Tasks.ReopenTask
{
    public class ReopenTaskHandler(
        ITaskItemRepository _taskItemRepository,
        IUnitOfWork _unitOfWork,
        IClock _clock
        )
    {
        public async Task Handle(ReopenTaskCommand command, CancellationToken ct)
        {
            var taksItem = await _taskItemRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Task not found.");

            taksItem.Reopen(_clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
