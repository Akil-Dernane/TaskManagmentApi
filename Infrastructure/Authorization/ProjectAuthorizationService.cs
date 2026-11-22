using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Authorization;
using Domain.Enums;
namespace Infrastructure.Authorization
{
    public class ProjectAuthorizationService(
        ICurrentUserService currentUserService,
        IProjectMembershipRepository projectMembershipRepository
        ) : IProjectAuthorizationService
    {

        public async Task<bool> HasPermissionAsync(Guid projectId, ProjectPermission permission, CancellationToken ct)
        {
            var memberId = currentUserService.MemberId;

            if (memberId == null)
                return false;

            var membership = await projectMembershipRepository
                .GetByProjectAndMemberAsync(projectId, memberId.Value, ct);

            if (membership == null)
                return false;

            return ProjectPermissionPolicy.IsGranted(
                membership.Role,
                permission);
        }
    }
}
