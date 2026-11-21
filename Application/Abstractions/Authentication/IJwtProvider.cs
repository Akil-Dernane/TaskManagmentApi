namespace Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        Task<AccessTokenResult> GenerateAccessTokenAsync( Guid userId, Guid memberId, string email, CancellationToken ct);
    }
}
