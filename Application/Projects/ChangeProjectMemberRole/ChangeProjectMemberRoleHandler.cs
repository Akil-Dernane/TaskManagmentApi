using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Enums;

namespace Application.Projects.ChangeProjectMemberRole
{
    public class ChangeProjectMemberRoleHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IProjectMembershipRepository projectMembershipRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task Handle(ChangeProjectMemberRoleCommand command, CancellationToken ct)
        {
            if (command.NewRole == ProjectRole.Owner)
                throw new InvalidOperationException("Ownership transfer must use a dedicated operation.");

            if (!await projectRepository.ExistsByIdAsync(command.ProjectId, ct))
                throw new KeyNotFoundException("Project not found");

            if (!await memberRepository.ExistsByIdAsync(command.MemberId, ct))
                throw new KeyNotFoundException("Member not found");

            var membership = await projectMembershipRepository.GetByProjectAndMemberAsync(command.ProjectId, command.MemberId, ct) 
                ?? throw new KeyNotFoundException("Membership doesn't exists.");

            if (membership.Role == ProjectRole.Owner)
                throw new InvalidOperationException("Can't change the owner's role.");

            membership.ChangeRole(command.NewRole);
            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}
