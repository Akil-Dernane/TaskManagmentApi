using API.Contracts.Tasks;
using Application.Tasks.ChangeTaskPriority;
using Application.Tasks.CompleteTask;
using Application.Tasks.CreateTaskItem;
using Application.Tasks.DTOs;
using Application.Tasks.GetTaskItemById;
using Application.Tasks.GetTasksByProjectId;
using Application.Tasks.ReopenTask;
using Application.Tasks.StartTask;
using Application.Tasks.UpdateTaskItem;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly CreateTaskItemHandler _createTaskItemHandler;
        private readonly GetTaskItemByIdHandler _getTaskItemByIdHandler;
        private readonly UpdateTaskItemHandler _updateTaskItemHandler;
        private readonly StartTaskHandler _startTaskHandler;
        private readonly CompleteTaskHandler _completeTaskHandler;
        private readonly ReopenTaskHandler _reopenTaskHandler;
        private readonly ChangeTaskPriorityHandler _changeTaskPriorityHandler;
        public TasksController(CreateTaskItemHandler createTaskItemHandler,
            GetTaskItemByIdHandler getTaskItemByIdHandler,
            UpdateTaskItemHandler updateTaskItemHandler,
            StartTaskHandler startTaskHandler,
            CompleteTaskHandler completeTaskHandler,
            ReopenTaskHandler reopenTaskHandler,
            ChangeTaskPriorityHandler changeTaskPriorityHandler)
        {
            _createTaskItemHandler = createTaskItemHandler;
            _getTaskItemByIdHandler = getTaskItemByIdHandler;
            _updateTaskItemHandler = updateTaskItemHandler;
            _startTaskHandler = startTaskHandler;
            _completeTaskHandler = completeTaskHandler;
            _reopenTaskHandler = reopenTaskHandler; 
            _changeTaskPriorityHandler = changeTaskPriorityHandler;
        }

        [HttpGet("{id:guid}")]
        public async Task<TaskItemDetailsDto> GetById(Guid id, CancellationToken ct)
        {
            return await _getTaskItemByIdHandler.Handle(new GetTaskItemByIdQuery(id), ct);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTaskItemRequest request, CancellationToken ct)
        {
            var taskId = await _createTaskItemHandler.Handle(new CreateTaskItemCommand(
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
            await _updateTaskItemHandler.Handle( new UpdateTaskItemCommand(id, request.Title, request.Description, request.DueDate) 
                , ct);
            return NoContent();
        }

        [HttpPatch("{id:guid}/start")]
        public async Task<IActionResult> Start(Guid id, CancellationToken ct)
        {
            await _startTaskHandler.Handle(new StartTaskCommand(id), ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
        {
            await _completeTaskHandler.Handle(new CompleteTaskCommand(id), ct);

            return NoContent();
        }
        [HttpPatch("{id:guid}/reopen")]
        public async Task<IActionResult> Reopen(Guid id, CancellationToken ct)
        {
            await _reopenTaskHandler.Handle(new ReopenTaskCommand(id), ct);

            return NoContent();
        }

        [HttpPatch("{id:guid}/priority")]
        public async Task<IActionResult> ChangePriority(Guid id, ChangeTaskPriorityRequest request, CancellationToken ct)
        {
            await _changeTaskPriorityHandler.Handle(new ChangeTaskPriorityCommand(id, request.Priority!.Value), ct);

            return NoContent();
        }
    }
}
