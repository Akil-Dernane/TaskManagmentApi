using Application.Abstractions.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly TaskManagementDbContext _context;

        public RefreshTokenRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken ct)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(
                    rt => rt.TokenHash == tokenHash,
                    ct);
        }

        public async Task<IReadOnlyList<RefreshToken>> GetByFamilyIdAsync(
            Guid familyId,
            CancellationToken ct)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.FamilyId == familyId)
                .ToListAsync(ct);
        }

        public void Add(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
        }

        public async Task<IReadOnlyList<RefreshToken>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        {

            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedAtUtc == null)
                .ToListAsync(ct);
        }
    }
}