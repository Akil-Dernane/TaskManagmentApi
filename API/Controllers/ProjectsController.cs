using API.Contracts.Projects;
using Application.Projects.AddProjectMember;
using Application.Projects.ArchiveProject;
using Application.Projects.ChangeProjectMemberRole;
using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Application.Projects.RemoveProjectMember;
using Application.Projects.TransferProjectOwnership;
using Application.Projects.UpdateProject;
using Application.Tasks.Enums;
using Application.Tasks.GetTasksByProjectId;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        private readonly AddProjectMemberHandler _addProjectMemberHandler;
        private readonly RemoveProjectMemberHandler _removeProjectMemberHandler;
        private readonly ChangeProjectMemberRoleHandler _changeProjectMemberRoleHandler;
        private readonly TransferProjectOwnershipHandler _transferProjectOwnershipHandler;
        public ProjectsController(
            CreateProjectHandler createProjectHandler,
            GetProjectByIdHandler getProjectByIdHandler,
            UpdateProjectHandler updateProjectHandler,
            ArchiveProjectHandler archiveProjectHandler,
            GetTasksByProjectIdHandler getTasksByProjectIdHandler,
            AddProjectMemberHandler addProjectMemberHandler,
            RemoveProjectMemberHandler removeProjectMemberHandler,
            ChangeProjectMemberRoleHandler changeProjectMemberRoleHandler,
            TransferProjectOwnershipHandler transferProjectOwnershipHandler)
        {
            _createProjectHandler = createProjectHandler;
            _getProjectByIdHandler = getProjectByIdHandler;
            _updateProjectHandler = updateProjectHandler;
            _archiveProjectHandler = archiveProjectHandler;
            _getTasksByProjectIdHandler = getTasksByProjectIdHandler;
            _addProjectMemberHandler = addProjectMemberHandler;
            _removeProjectMemberHandler = removeProjectMemberHandler;
            _changeProjectMemberRoleHandler = changeProjectMemberRoleHandler;
            _transferProjectOwnershipHandler = transferProjectOwnershipHandler;
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
            [FromQuery, EnumDataType(typeof(ProjectTaskPriority))]
            ProjectTaskPriority? priority = null,

            [FromQuery, EnumDataType(typeof(ProjectTaskStatus))]
            ProjectTaskStatus? status = null,

            [FromQuery, Range(1, int.MaxValue)]
                int page = 1,

            [FromQuery, Range(1, 100)]
            int pageSize = 10,

            [FromQuery, EnumDataType(typeof(TaskSortBy))]
            TaskSortBy sortBy = TaskSortBy.CreatedAt,

            [FromQuery, EnumDataType(typeof(SortDirection))]
            SortDirection sortDirection = SortDirection.Descending)
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

        [HttpPost("{projectId:guid}/members")]
        public async Task<IActionResult> AddProjectMember(Guid projectId, AddProjectMemberRequest request, CancellationToken ct)
        {
            await _addProjectMemberHandler.Handle(new AddProjectMemberCommand(projectId, request.MemberId!.Value, request.Role!.Value), ct);

            return NoContent();
        }

        [HttpDelete("{projectId:guid}/members/{memberId:guid}")]
        public async Task<IActionResult> RemoveProjectMember(Guid projectId, Guid memberId, CancellationToken ct)
        {
            await _removeProjectMemberHandler.Handle(new RemoveProjectMemberCommand(projectId, memberId), ct);

            return NoContent();
        }

        [HttpPut("{projectId:guid}/members/{memberId:guid}/role")]
        public async Task<IActionResult> ChangeProjectMemberRole(
            Guid projectId, 
            Guid memberId, 
            ChangeProjectMemberRoleRequest request, 
            CancellationToken ct)
        {
            await _changeProjectMemberRoleHandler.Handle(new ChangeProjectMemberRoleCommand(projectId, memberId, request.Role!.Value), ct);

            return NoContent();
        }

        [HttpPut("{projectId:guid}/transfer-ownership")]
        public async Task<IActionResult> TransferOwnership(Guid projectId, TransferProjectOwnershipRequest request, CancellationToken ct)
        {
            await _transferProjectOwnershipHandler.Handle(
                new TransferProjectOwnershipCommand(projectId, request.NewOwnerMemberId!.Value),
                ct);

            return NoContent();
        }
    }
}
