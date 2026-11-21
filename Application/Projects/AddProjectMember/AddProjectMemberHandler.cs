using Application.Abstractions;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Projects.AddProjectMember
{
    public class AddProjectMemberHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IProjectMembershipRepository projectMembershipRepository,
        IProjectAuthorizationService projectAuthorizationService,
        IClock clock,
        IUnitOfWork unitOfWork)
    {
        public async Task Handle(AddProjectMemberCommand command, CancellationToken ct)
        {
            if (command.Role == ProjectRole.Owner)
                throw new InvalidOperationException("A project can only have one owner.");

            if (!await projectRepository.ExistsByIdAsync(command.ProjectId, ct))
                throw new KeyNotFoundException("Project not found.");

            var isOwner = await projectAuthorizationService.HasRoleAsync(
                command.ProjectId, ProjectRole.Owner, ct);

            var isManager = await projectAuthorizationService.HasRoleAsync(
                command.ProjectId, ProjectRole.Manager, ct);

            if (!isOwner && !isManager)
                throw new ForbiddenAccessException();

            if (!await memberRepository.ExistsByIdAsync(command.MemberId, ct))
                throw new KeyNotFoundException("Member not found.");

            if (await projectMembershipRepository.ExistsAsync(
                command.ProjectId, command.MemberId, ct))
            {
                throw new ConflictException("Membership already exists.");
            }

            var projectMembership = new ProjectMembership(
                command.ProjectId,
                command.MemberId,
                command.Role,
                clock.UtcNow);

            await projectMembershipRepository.AddAsync(projectMembership, ct);

            await unitOfWork.SaveChangesAsync(ct);
        }
    }
}