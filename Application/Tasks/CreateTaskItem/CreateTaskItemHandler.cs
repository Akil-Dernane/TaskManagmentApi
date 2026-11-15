using Application.Abstractions;
using Application.Abstractions.Persistence;
using Domain.Entities;

namespace Application.Tasks.CreateTaskItem
{
    public class CreateTaskItemHandler (
        ITaskItemRepository _taskItemRepository,
        IProjectRepository _projectRepository,
        IUnitOfWork _unitOfWork,
        IClock _clock )
    {
        public async Task<Guid> Handle(CreateTaskItemCommand command, CancellationToken ct)
        {
            var project = await _projectRepository.GetByIdAsync(command.ProjectId, ct) ?? throw new KeyNotFoundException("Project not found");

            var now = _clock.UtcNow;

            var taskItem = new TaskItem(
                command.Title, 
                now, 
                command.ProjectId, 
                command.Description, 
                command.DueDate);

            await _taskItemRepository.AddAsync(taskItem, ct);
            await _unitOfWork.SaveChangesAsync( ct );

            return taskItem.Id;
        }
    }
}
