using Domain.Enums;

namespace Application.Abstractions.Authorization
{
    public interface IProjectAuthorizationService
    {
        Task<bool> IsMemberAsync(Guid projectId, CancellationToken ct);

        Task<bool> HasRoleAsync(Guid projectId, ProjectRole role, CancellationToken ct);
    }
}