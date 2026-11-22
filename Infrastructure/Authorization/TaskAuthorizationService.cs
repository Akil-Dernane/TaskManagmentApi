using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Application.Abstractions.Persistence;
using Application.Authorization;
using Domain.Enums;

namespace Infrastructure.Authorization
{
    public class TaskAuthorizationService(
        ICurrentUserService currentUserService,
        IProjectMembershipRepository projectMembershipRepository
        ) : ITaskAuthorizationService
    {
        public async Task<bool> HasPermissionAsync(Guid projectId, TaskPermission permission, CancellationToken ct)
        {
            var memberId = currentUserService.MemberId;

            if (memberId == null)
                return false;

            var membership = await projectMembershipRepository
                .GetByProjectAndMemberAsync(projectId, memberId.Value, ct);

            if (membership == null)
                return false;

            return TaskPermissionPolicy.IsGranted(
                membership.Role,
                permission);
        }

        public async Task<bool> CanUpdateTaskAsync(Guid projectId, Guid? assignedMemberId, CancellationToken ct)
        {
            var memberId = currentUserService.MemberId;
            if (memberId == null) return false;

            var membership = await projectMembershipRepository
                .GetByProjectAndMemberAsync(projectId, memberId.Value, ct);

            if (membership == null) return false;

            bool garanted = TaskPermissionPolicy.IsGranted(
                membership.Role,
                TaskPermission.UpdateAssignedTasks);

            bool allowed = true;
            if (membership.Role == ProjectRole.Member)
                allowed = assignedMemberId == memberId.Value;

            return garanted && allowed;
        }
    }
}