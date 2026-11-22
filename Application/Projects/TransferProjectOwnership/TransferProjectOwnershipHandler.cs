using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Projects.TransferProjectOwnership
{
    public class TransferProjectOwnershipHandler(
        IProjectRepository projectRepository,
        IProjectMembershipRepository projectMembershipRepository,
        IProjectAuthorizationService projectAuthorizationService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        public async Task Handle(
            TransferProjectOwnershipCommand command,
            CancellationToken ct)
        {
            if (command.NewOwnerMemberId == Guid.Empty)
                throw new ArgumentException("New owner member ID is required.");

            if (!await projectRepository.ExistsByIdAsync(command.ProjectId, ct))
                throw new KeyNotFoundException("Project not found.");

            if (!await projectAuthorizationService.HasRoleAsync(
                command.ProjectId, ProjectRole.Owner, ct))
                throw new ForbiddenAccessException();

            var currentMemberId = currentUserService.MemberId
                ?? throw new UnauthorizedException();

            var currentOwner = await projectMembershipRepository
                .GetByProjectAndMemberAsync(command.ProjectId, currentMemberId, ct)
                ?? throw new KeyNotFoundException("Owner membership not found.");

            if (currentMemberId == command.NewOwnerMemberId)
                throw new InvalidOperationException(
                    "The current owner is already the project owner.");

            var newOwner = await projectMembershipRepository
                .GetByProjectAndMemberAsync(
                    command.ProjectId,
                    command.NewOwnerMemberId,
                    ct)
                 ?? throw new ConflictException(
                    "The new owner must already be a project member.");

            currentOwner.ChangeRole(ProjectRole.Member);
            newOwner.ChangeRole(ProjectRole.Owner);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}