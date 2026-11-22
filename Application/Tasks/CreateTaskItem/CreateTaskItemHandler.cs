using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Tasks.CreateTaskItem
{
    public class CreateTaskItemHandler(
        ITaskItemRepository taskItemRepository,
        IProjectRepository projectRepository,
        ITaskAuthorizationService taskAuthorizationService,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        public async Task<Guid> Handle(
            CreateTaskItemCommand command,
            CancellationToken ct)
        {
            var project = await projectRepository.GetByIdAsync(command.ProjectId, ct)
                ?? throw new KeyNotFoundException("Project not found.");

            if (!await taskAuthorizationService.HasPermissionAsync(
                command.ProjectId, TaskPermission.CreateTasks, ct))
                throw new ForbiddenAccessException();

            var now = clock.UtcNow;

            var taskItem = new TaskItem(
                command.Title,
                now,
                command.ProjectId,
                command.Description,
                command.DueDate);

            await taskItemRepository.AddAsync(taskItem, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return taskItem.Id;
        }
    }
}