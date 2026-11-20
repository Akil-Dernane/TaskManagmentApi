using Application.Abstractions.Authentication;

namespace Application.Authentication.Register
{
    public class RegisterUserHandler(
        IIdentityService identityService,
        IJwtProvider jwtProvider,
        IRefreshTokenService refreshTokenService)
    {
        public async Task<AuthenticationResult> Handle(
            RegisterUserCommand command,
            CancellationToken ct)
        {
            var userId = await identityService.CreateUserAsync(
                command.Email,
                command.Password,
                ct);

            var accessToken = await jwtProvider.GenerateAccessTokenAsync(
                userId,
                command.Email,
                ct);

            var refreshToken = await refreshTokenService.CreateAsync(
                userId,
                null,
                ct);

            return new AuthenticationResult(
                accessToken.AccessToken,
                accessToken.ExpiresAtUtc,
                refreshToken.Token,
                refreshToken.ExpiresAtUtc);
        }
    }
}