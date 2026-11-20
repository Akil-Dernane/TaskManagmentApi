using Domain.Entities;

namespace Application.Abstractions.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken ct);

        Task<IReadOnlyList<RefreshToken>> GetByFamilyIdAsync(
            Guid familyId,
            CancellationToken ct);

        void Add(RefreshToken refreshToken);

        Task<IReadOnlyList<RefreshToken>> GetByUserIdAsync(
            Guid userId,
            CancellationToken ct);
    }
}