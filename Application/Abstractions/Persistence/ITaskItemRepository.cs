using Application.Common.DTOs;
using Application.Tasks.DTOs;
using Application.Tasks.Enums;
using Domain.Entities;
using Domain.Enums;

namespace Application.Abstractions.Persistence
{
    public interface ITaskItemRepository
    {
        Task AddAsync(TaskItem taskItem, CancellationToken ct);
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<TaskItemDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct);
        Task<PagedResult<TaskItemDetailsDto>> GetByProjectIdAsync(
            Guid projectId,
            ProjectTaskPriority? priority,
            ProjectTaskStatus? status,
            int page,
            int pageSize,
            TaskSortBy sortBy,
            SortDirection sortDirection,
            CancellationToken ct);
    }
}
