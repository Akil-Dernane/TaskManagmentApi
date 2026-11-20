namespace Application.Abstractions.Authentication
{
    public record AccessTokenResult(
        string AccessToken,
        DateTime ExpiresAtUtc);
}