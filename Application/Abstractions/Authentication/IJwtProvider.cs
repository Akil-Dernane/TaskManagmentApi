namespace Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        Task<AccessTokenResult> GenerateAccessTokenAsync( Guid userId, string email, CancellationToken ct);
    }
}
