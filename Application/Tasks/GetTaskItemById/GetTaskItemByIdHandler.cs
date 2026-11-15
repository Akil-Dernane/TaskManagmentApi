using Application.Abstractions.Persistence;
using Application.Tasks.DTOs;

namespace Application.Tasks.GetTaskItemById
{
    public class GetTaskItemByIdHandler(ITaskItemRepository _taskItemRepository)
    {
        public async Task<TaskItemDetailsDto> Handle(GetTaskItemByIdQuery query, CancellationToken ct)
        {
            var taskItem = await _taskItemRepository.GetDetailsByIdAsync(query.Id, ct) ?? throw new KeyNotFoundException("Task not found.");
            return taskItem;
        }
    }
}
