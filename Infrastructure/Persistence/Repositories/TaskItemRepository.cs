using Application.Abstractions.Persistence;
using Application.Common.DTOs;
using Application.Tasks.DTOs;
using Application.Tasks.Enums;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskManagementDbContext _dbContext;
        public TaskItemRepository(TaskManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TaskItem taskItem, CancellationToken ct)
        {
            await _dbContext.TaskItems.AddAsync(taskItem, ct);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.TaskItems.FirstOrDefaultAsync(t =>  t.Id == id, ct);
        }

        public async Task<PagedResult<TaskItemDetailsDto>> GetByProjectIdAsync(
            Guid projectId,
            ProjectTaskPriority? priority,
            ProjectTaskStatus? status,
            int page,
            int pageSize,
            TaskSortBy sortBy,
            SortDirection sortDirection,
            CancellationToken ct)
        {
            var query = _dbContext.TaskItems
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId);

            if(priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if(status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            var totalCount = await query.CountAsync(ct);

            query = (sortBy, sortDirection) switch
            {
                (TaskSortBy.CreatedAt, SortDirection.Ascending)
                    => query.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id),

                (TaskSortBy.CreatedAt, SortDirection.Descending)
                    => query.OrderByDescending(t => t.CreatedAt).ThenBy(t => t.Id),

                (TaskSortBy.DueDate, SortDirection.Ascending)
                    => query.OrderBy(t => t.DueDate).ThenBy(t => t.Id),

                (TaskSortBy.DueDate, SortDirection.Descending)
                    => query.OrderByDescending(t => t.DueDate).ThenBy(t => t.Id),

                (TaskSortBy.Priority, SortDirection.Ascending)
                    => query.OrderBy(t => t.Priority).ThenBy(t => t.Id),

                (TaskSortBy.Priority, SortDirection.Descending)
                    => query.OrderByDescending(t => t.Priority).ThenBy(t => t.Id),

                (TaskSortBy.Title, SortDirection.Ascending)
                    => query.OrderBy(t => t.Title).ThenBy(t => t.Id),

                (TaskSortBy.Title, SortDirection.Descending)
                    => query.OrderByDescending(t => t.Title).ThenBy(t => t.Id),

                _ => throw new ArgumentOutOfRangeException(nameof(sortBy))
            };

            var skip = (int)Math.Min(((long)page - 1) * pageSize, int.MaxValue);

            query = query.Skip(skip).Take(pageSize);

            var results = await query
                .Select(t => new TaskItemDetailsDto(
                    t.Id, t.ProjectId, t.AssignedMemberId, t.Title, t.Description,
                    t.Status, t.Priority, t.DueDate, t.CompletedAt,
                    t.CreatedAt, t.UpdatedAt))
                .ToListAsync(ct);

            return new PagedResult<TaskItemDetailsDto>(results,page,pageSize,totalCount);
        }

        public async Task<TaskItemDetailsDto?> GetDetailsByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbContext.TaskItems
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TaskItemDetailsDto(
                    t.Id, t.ProjectId, t.AssignedMemberId, t.Title, t.Description, 
                    t.Status, t.Priority, t.DueDate, t.CompletedAt, 
                    t.CreatedAt, t.UpdatedAt))
                .FirstOrDefaultAsync(ct);
        }
    }
}
