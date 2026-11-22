
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Projects.ArchiveProject
{
    public class ArchiveProjectHandler
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectAuthorizationService _projectAuthorizationService;
        private readonly IUnitOfWork _unitOfWork;
        public ArchiveProjectHandler(
            IProjectRepository projectRepository,
            IProjectAuthorizationService projectAuthorizationService,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _projectAuthorizationService = projectAuthorizationService;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ArchiveProjectCommand command, CancellationToken ct)
        {
            var project = await _projectRepository.GetByIdAsync(command.Id, ct) ?? throw new KeyNotFoundException("Project not found.");

            if (!await _projectAuthorizationService.HasPermissionAsync(
                command.Id, ProjectPermission.ArchiveProject, ct))
                            throw new ForbiddenAccessException();

            project.Archive();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
