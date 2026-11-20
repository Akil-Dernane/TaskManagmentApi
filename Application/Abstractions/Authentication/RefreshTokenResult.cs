namespace Application.Abstractions.Authentication
{
    public record RefreshTokenResult(
        string Token,
        DateTime ExpiresAtUtc);
}