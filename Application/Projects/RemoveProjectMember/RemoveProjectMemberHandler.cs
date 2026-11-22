using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Projects.RemoveProjectMember
{
    public class RemoveProjectMemberHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IProjectMembershipRepository projectMembershipRepository,
        IProjectAuthorizationService projectAuthorizationService,
        IUnitOfWork unitOfWork)
    {
        public async Task Handle(RemoveProjectMemberCommand command, CancellationToken ct)
        {
            if (!await projectRepository.ExistsByIdAsync(command.ProjectId, ct))
                throw new KeyNotFoundException("Project not found.");

            if (!await projectAuthorizationService.HasPermissionAsync(
                command.ProjectId, ProjectPermission.ManageMembers, ct))
                            throw new ForbiddenAccessException();

            if (!await memberRepository.ExistsByIdAsync(command.MemberId, ct))
                throw new KeyNotFoundException("Member not found.");

            var membership = await projectMembershipRepository.GetByProjectAndMemberAsync(
                command.ProjectId,
                command.MemberId,
                ct) ?? throw new KeyNotFoundException("Membership doesn't exist.");

            if (membership.Role == ProjectRole.Owner)
                throw new InvalidOperationException("Can't remove the owner membership.");

            projectMembershipRepository.Remove(membership);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}