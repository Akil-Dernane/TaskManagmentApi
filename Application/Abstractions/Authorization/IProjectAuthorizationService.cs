using Domain.Enums;

namespace Application.Abstractions.Authorization
{
    public interface IProjectAuthorizationService
    {
        Task<bool> HasPermissionAsync(Guid projectId, ProjectPermission permission, CancellationToken ct);
    }
}