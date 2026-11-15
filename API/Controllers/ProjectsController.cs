using API.Contracts.Projects;
using Application.Projects.ArchiveProject;
using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Application.Projects.UpdateProject;
using Application.Tasks.Enums;
using Application.Tasks.GetTasksByProjectId;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly CreateProjectHandler _createProjectHandler;
        private readonly GetProjectByIdHandler _getProjectByIdHandler;
        private readonly UpdateProjectHandler _updateProjectHandler;
        private readonly ArchiveProjectHandler _archiveProjectHandler;
        private readonly GetTasksByProjectIdHandler _getTasksByProjectIdHandler;
        public ProjectsController(
            CreateProjectHandler createProjectHandler,
            GetProjectByIdHandler getProjectByIdHandler,
            UpdateProjectHandler updateProjectHandler,
            ArchiveProjectHandler archiveProjectHandler,
            GetTasksByProjectIdHandler getTasksByProjectIdHandler)
        {
            _createProjectHandler = createProjectHandler;
            _getProjectByIdHandler = getProjectByIdHandler;
            _updateProjectHandler = updateProjectHandler;
            _archiveProjectHandler = archiveProjectHandler;
            _getTasksByProjectIdHandler = getTasksByProjectIdHandler;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken ct)
        {
            var project = await _getProjectByIdHandler.Handle(
                new GetProjectByIdQuery(id), ct);

            return Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProjectRequest request,
            CancellationToken ct)
        {
            var projectId = await _createProjectHandler.Handle(
                new CreateProjectCommand(request.Name, request.Description), ct);

            return CreatedAtAction(
                nameof(GetById),
                new { id = projectId },
                new { id = projectId });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateProjectRequest request, CancellationToken ct)
        {
            await _updateProjectHandler.Handle(new UpdateProjectCommand(id, request.Name, request.Description),
                ct);
            return NoContent();
        }

        [HttpPatch("{id:guid}/archive")]
        public async Task<IActionResult> Archive(Guid id,CancellationToken ct)
        {
            await _archiveProjectHandler.Handle(new ArchiveProjectCommand(id),
                ct);

            return NoContent();
        }

        [HttpGet("{projectId:guid}/tasks")]
        public async Task<ActionResult> GetTasksByProjectId(
            Guid projectId,
            CancellationToken ct,
            [FromQuery] ProjectTaskPriority? priority = null,
            [FromQuery] ProjectTaskStatus? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] TaskSortBy sortBy = TaskSortBy.CreatedAt,
            [FromQuery] SortDirection sortDirection = SortDirection.Descending)
        {
            var tasks = await _getTasksByProjectIdHandler.Handle(new GetTasksByProjectIdQuery(
                projectId, 
                priority, 
                status,
                page,
                pageSize,
                sortBy,
                sortDirection
                ), ct);
            return Ok(tasks);
        }
    }
}
