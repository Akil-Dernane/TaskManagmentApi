namespace Application.Abstractions.Authentication
{
    public record AuthenticationResult(
        string AccessToken,
        DateTime AccessTokenExpiresAtUtc,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc);
}