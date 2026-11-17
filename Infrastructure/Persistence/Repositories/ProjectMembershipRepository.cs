using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProjectMembershipRepository(TaskManagementDbContext _dbContext) : IProjectMembershipRepository
    {
        public async Task AddAsync(ProjectMembership membership, CancellationToken ct)
        {
            await _dbContext.ProjectMemberships.AddAsync(membership, ct);
        }

        public async Task<bool> ExistsAsync(Guid projectId, Guid memberId, CancellationToken ct)
        {
            return await _dbContext.ProjectMemberships
                .AnyAsync(m => m.ProjectId == projectId &&
                               m.MemberId == memberId, ct );     
        }

        public async Task<ProjectMembership?> GetByProjectAndMemberAsync(Guid projectId, Guid memberId, CancellationToken ct)
        {
            return await _dbContext.ProjectMemberships.FirstOrDefaultAsync( m => m.ProjectId == projectId && m.MemberId == memberId , ct);
        }

        public void Remove(ProjectMembership membership)
        {
            _dbContext.ProjectMemberships.Remove(membership);
        }
    }
}
