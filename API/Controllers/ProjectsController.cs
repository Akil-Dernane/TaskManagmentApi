using API.Contracts.Projects;
using Application.Projects.CreateProject;
using Application.Projects.GetProjectById;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly CreateProjectHandler _createProjectHandler;
        private readonly GetProjectByIdHandler _getProjectByIdHandler;
        public ProjectsController(CreateProjectHandler createProjectHandler,GetProjectByIdHandler getProjectByIdHandler)
        {
            _createProjectHandler = createProjectHandler;
            _getProjectByIdHandler = getProjectByIdHandler;
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
    }
}
