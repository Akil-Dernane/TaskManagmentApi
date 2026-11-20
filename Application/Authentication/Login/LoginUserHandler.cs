using Application.Abstractions.Authentication;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authentication.Login
{
    public class LoginUserHandler(
        IIdentityService identityService,
        IRefreshTokenService refreshTokenService,
        IJwtProvider jwtProvider)
    {
        public async Task<AuthenticationResult> Handle(LoginUserCommand command, CancellationToken ct)
        {
            var result = await identityService.ValidateCredentialsAsync(
                command.Email,
                command.Password,
                ct);

            switch (result.Status)
            {
                case CredentialValidationStatus.Success:
                    {
                        var userId = result.UserId!.Value;

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

                case CredentialValidationStatus.InvalidCredentials:
                    throw new UnauthorizedException();

                case CredentialValidationStatus.LockedOut:
                    throw new AccountLockedException();

                case CredentialValidationStatus.NotAllowed:
                    throw new ForbiddenException();

                default:
                    throw new InvalidOperationException(
                        "Unknown credential validation status.");
            }
        }
    }
}