using Domain.Enums;

namespace Application.Abstractions.Authorization
{
    public interface ITaskAuthorizationService
    {
        Task<bool> HasPermissionAsync(Guid projectId,TaskPermission permission,CancellationToken ct);
        Task<bool> CanUpdateTaskAsync(Guid projectId, Guid? assignedMemberId, CancellationToken ct);
    }
}