namespace Application.Abstractions.Authentication
{
    public record RefreshTokenRotationResult(
        Guid UserId,
        RefreshTokenResult RefreshToken);
}