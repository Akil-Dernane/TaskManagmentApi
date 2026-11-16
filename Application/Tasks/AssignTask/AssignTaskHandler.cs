using Application.Abstractions;
using Application.Abstractions.Persistence;

namespace Application.Tasks.AssignTask
{
    public class AssignTaskHandler(
        ITaskItemRepository _taskItemRepository, 
        IMemberRepository _memberRepository,
        IClock _clock, 
        IUnitOfWork _unitOfWork)
    {
        public async Task Handle(AssignTaskCommand command, CancellationToken ct)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(command.TaskId, ct) ?? throw new KeyNotFoundException("Task not found");

            if(!await _memberRepository.ExistsByIdAsync(command.MemberId, ct)) throw new KeyNotFoundException("Member not found.");

            taskItem.Assign(command.MemberId, _clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
