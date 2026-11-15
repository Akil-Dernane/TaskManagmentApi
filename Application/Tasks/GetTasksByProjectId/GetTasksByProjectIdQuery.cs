using Application.Tasks.Enums;
using Domain.Enums;

namespace Application.Tasks.GetTasksByProjectId
{
    public record GetTasksByProjectIdQuery(
        Guid ProjectId,
        ProjectTaskPriority? Priority = null,
        ProjectTaskStatus? Status = null,
        int Page = 1,
        int PageSize = 10,
        TaskSortBy SortBy = TaskSortBy.CreatedAt,
        SortDirection SortDirection = SortDirection.Descending);
}
