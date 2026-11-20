using Application.Abstractions.Authentication;
using Application.Exceptions;

namespace Application.Authentication.Refresh
{
    public class RefreshTokenHandler(
        IRefreshTokenService refreshTokenService,
        IJwtProvider jwtProvider,
        IIdentityService identityService)
    {
        public async Task<AuthenticationResult> Handle(
            RefreshTokenCommand command,
            CancellationToken ct)
        {
            var rotationResult = await refreshTokenService.RotateAsync(
                command.RefreshToken,
                ct);

            var email = await identityService.GetEmailAsync(
                rotationResult.UserId,
                ct);

            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedException();

            var accessToken = await jwtProvider.GenerateAccessTokenAsync(
                rotationResult.UserId,
                email,
                ct);

            return new AuthenticationResult(
                accessToken.AccessToken,
                accessToken.ExpiresAtUtc,
                rotationResult.RefreshToken.Token,
                rotationResult.RefreshToken.ExpiresAtUtc);
        }
    }
}