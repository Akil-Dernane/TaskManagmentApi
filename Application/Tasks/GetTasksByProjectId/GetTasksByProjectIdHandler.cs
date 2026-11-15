using Application.Abstractions.Persistence;
using Application.Common.DTOs;
using Application.Tasks.DTOs;

namespace Application.Tasks.GetTasksByProjectId
{
    public class GetTasksByProjectIdHandler(ITaskItemRepository _taskItemRepository,
        IProjectRepository _projectRepository)
    {
        public async Task<PagedResult<TaskItemDetailsDto>> Handle(GetTasksByProjectIdQuery query, CancellationToken ct)
        {
            if (!await _projectRepository.ExistsByIdAsync(query.ProjectId, ct))
                throw new KeyNotFoundException("Project not found.");

            return await _taskItemRepository.GetByProjectIdAsync(
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
