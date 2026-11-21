
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Projects.CreateProject
{
    public class CreateProjectHandler
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMembershipRepository _projectMembershipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        public CreateProjectHandler(
            ICurrentUserService currentUserService,
            IProjectRepository projectRepository,
            IProjectMembershipRepository projectMembershipRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _currentUserService = currentUserService;
            _projectRepository = projectRepository;
            _projectMembershipRepository = projectMembershipRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task<Guid> Handle(CreateProjectCommand command, CancellationToken ct)
        {
            var memberId = _currentUserService.MemberId
                ?? throw new UnauthorizedException();

            if (await _projectRepository.ExistsByNameAsync(command.Name, ct))
                throw new ConflictException("A project with this name already exists.");

            var now = _clock.UtcNow;
            var project = new Project(command.Name, command.Description, now);

            var membership = new ProjectMembership(
                project.Id,
                memberId,
                ProjectRole.Owner,
                now);
            await _projectMembershipRepository.AddAsync(membership, ct);

            await _projectRepository.AddAsync(project, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return project.Id;
        }
    }
}
