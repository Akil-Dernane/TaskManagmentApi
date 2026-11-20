namespace Application.Abstractions.Authentication
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResult> CreateAsync(
            Guid userId,
            Guid? familyId,
            CancellationToken ct);

        Task<RefreshTokenRotationResult> RotateAsync(
            string token,
            CancellationToken ct);

        Task RevokeFamilyAsync(
            string token,
            CancellationToken ct);

        Task RevokeAllAsync(
            Guid userId,
            CancellationToken ct);
    }
}