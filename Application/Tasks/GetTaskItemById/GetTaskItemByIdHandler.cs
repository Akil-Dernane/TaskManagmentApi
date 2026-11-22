using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Application.Tasks.DTOs;
using Domain.Enums;

namespace Application.Tasks.GetTaskItemById
{
    public class GetTaskItemByIdHandler(
        ITaskItemRepository taskItemRepository,
        ITaskAuthorizationService taskAuthorizationService)
    {
        public async Task<TaskItemDetailsDto> Handle(
            GetTaskItemByIdQuery query,
            CancellationToken ct)
        {
            var taskItem = await taskItemRepository.GetDetailsByIdAsync(query.Id, ct)
                ?? throw new KeyNotFoundException("Task not found.");

            if (!await taskAuthorizationService.HasPermissionAsync(
                taskItem.ProjectId,
                TaskPermission.ViewTasks,
                ct))
                throw new ForbiddenAccessException();

            return taskItem;
        }
    }
}