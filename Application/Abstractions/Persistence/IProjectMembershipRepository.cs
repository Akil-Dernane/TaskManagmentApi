using Domain.Entities;

namespace Application.Abstractions.Persistence
{
    public interface IProjectMembershipRepository
    {
        Task AddAsync(ProjectMembership membership, CancellationToken ct);
        Task<bool> ExistsAsync(Guid projectId, Guid memberId, CancellationToken ct);
        Task<ProjectMembership?> GetByProjectAndMemberAsync(Guid projectId, Guid memberId, CancellationToken ct);
        void Remove(ProjectMembership membership);
    }
}
