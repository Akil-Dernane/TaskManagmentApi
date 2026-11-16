using API.Contracts.Tasks;
using Application.Tasks.AssignTask;
using Application.Tasks.ChangeTaskPriority;
using Application.Tasks.CompleteTask;
using Application.Tasks.CreateTaskItem;
using Application.Tasks.DTOs;
using Application.Tasks.GetTaskItemById;
using Application.Tasks.GetTasksByProjectId;
using Application.Tasks.ReopenTask;
using Application.Tasks.StartTask;
using Application.Tasks.UnassignTask;
using Application.Tasks.UpdateTaskItem;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController(
        CreateTaskItemHandler createTaskItemHandler,
        GetTaskItemByIdHandler getTaskItemByIdHandler,
        UpdateTaskItemHandler updateTaskItemHandler,
        StartTaskHandler startTaskHandler,
        CompleteTaskHandler completeTaskHandler,
        ReopenTaskHandler reopenTaskHandler,
        ChangeTaskPriorityHandler changeTaskPriorityHandler,
        AssignTaskHandler assignTaskHandler,
        UnassignTaskHandler unassignTaskHandler
    ) : ControllerBase
    {

        [HttpGet("{id:guid}")]
        public async Task<TaskItemDetailsDto> GetById(Guid id, CancellationToken ct)
        {
            return await getTaskItemByIdHandler.Handle(new GetTaskItemByIdQuery(id), ct);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTaskItemRequest request, CancellationToken ct)
        {
            var taskId = await createTaskItemHandler.Handle(new CreateTaskItemCommand(
                request.ProjectId,
                request.Title,
                request.Description,
                request.DueDate) , ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = taskId },
                new { id = taskId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskItemRequest request, CancellationToken ct)
        {
            await updateTaskItemHandler.Handle( new UpdateTaskItemCommand(id, request.Title, request.Description, request.DueDate) 
                , ct);
            return NoContent();
        }

        [HttpPatch("{id:guid}/start")]
        public async Task<IActionResult> Start(Guid id, CancellationToken ct)
        {
            await startTaskHandler.Handle(new StartTaskCommand(id), ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
        {
            await completeTaskHandler.Handle(new CompleteTaskCommand(id), ct);

            return NoContent();
        }
        [HttpPatch("{id:guid}/reopen")]
        public async Task<IActionResult> Reopen(Guid id, CancellationToken ct)
        {
            await reopenTaskHandler.Handle(new ReopenTaskCommand(id), ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/priority")]
        public async Task<IActionResult> ChangePriority(Guid id, ChangeTaskPriorityRequest request, CancellationToken ct)
        {
            await changeTaskPriorityHandler.Handle(new ChangeTaskPriorityCommand(id, request.Priority!.Value), ct);

            return NoContent();
        }

        [HttpPut("{taskId:guid}/assignee")]
        public async Task<IActionResult> Assign(Guid taskId, AssignTaskRequest request , CancellationToken ct)
        {
            await assignTaskHandler.Handle(new AssignTaskCommand(taskId, request.MemberId!.Value), ct);
            return NoContent();
        }

        [HttpDelete("{taskId:guid}/assignee")]
        public async Task<IActionResult> Unassign(Guid taskId, CancellationToken ct)
        {
            await unassignTaskHandler.Handle( new UnassignTaskCommand(taskId), ct);

            return NoContent();
        }
    }
}
