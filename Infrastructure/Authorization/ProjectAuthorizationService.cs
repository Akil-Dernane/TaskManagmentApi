using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Domain.Enums;
namespace Infrastructure.Authorization
{
    public class ProjectAuthorizationService(
        ICurrentUserService currentUserService,
        IProjectMembershipRepository projectMembershipRepository
        ) : IProjectAuthorizationService
    {
        public async Task<bool> HasRoleAsync(Guid projectId, ProjectRole role, CancellationToken ct)
        {
            var memberId = currentUserService.MemberId;

            if (memberId == null) return false;

            var membership = await projectMembershipRepository.GetByProjectAndMemberAsync(projectId, memberId.Value, ct);

            return membership?.Role == role;
        }

        public async Task<bool> IsMemberAsync(Guid projectId, CancellationToken ct)
        {
            var memberId = currentUserService.MemberId;

            if( memberId == null ) return false;

            return await projectMembershipRepository.ExistsAsync(projectId, memberId.Value, ct);
        }
    }
}
