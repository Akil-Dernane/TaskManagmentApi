using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Common.DTOs;
using Application.Exceptions;
using Application.Tasks.DTOs;
using Domain.Enums;

namespace Application.Tasks.GetTasksByProjectId
{
    public class GetTasksByProjectIdHandler(
        ITaskItemRepository taskItemRepository,
        IProjectRepository projectRepository,
        ITaskAuthorizationService taskAuthorizationService)
    {
        public async Task<PagedResult<TaskItemDetailsDto>> Handle(
            GetTasksByProjectIdQuery query,
            CancellationToken ct)
        {
            if (!await projectRepository.ExistsByIdAsync(query.ProjectId, ct))
                throw new KeyNotFoundException("Project not found.");

            if (!await taskAuthorizationService.HasPermissionAsync(
                query.ProjectId,
                TaskPermission.ViewTasks,
                ct))
                throw new ForbiddenAccessException();

            return await taskItemRepository.GetByProjectIdAsync(
                query.ProjectId,
                query.Priority,
                query.Status,
                query.Page,
                query.PageSize,
                query.SortBy,
                query.SortDirection,
                ct);
        }
    }
}