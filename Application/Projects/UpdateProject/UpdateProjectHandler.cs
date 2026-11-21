using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;
namespace Application.Projects.UpdateProject
{
    public class UpdateProjectHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectHandler(
            IProjectRepository projectRepository,
            IProjectAuthorizationService projectAuthorizationService,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProjectCommand command, CancellationToken ct)
        {
            var project = await _projectRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Project not found.");

            var isOwner = await _projectAuthorizationService.HasRoleAsync(
                command.Id, ProjectRole.Owner, ct);

            var isManager = await _projectAuthorizationService.HasRoleAsync(
                command.Id, ProjectRole.Manager, ct);

            if (!isOwner && !isManager)
                throw new ForbiddenAccessException();

            var nameExists = await _projectRepository.ExistsByNameAsync(command.Name, ct);

            if (nameExists && !string.Equals(project.Name,command.Name,StringComparison.OrdinalIgnoreCase))
            {
                throw new ConflictException(
                "A project with this name already exists.");
            }

            project.UpdateDetails(command.Name, command.Description);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
