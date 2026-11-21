using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using Application.Exceptions;

namespace Application.Authentication.Login
{
    public class LoginUserHandler(
        IIdentityService identityService,
        IRefreshTokenService refreshTokenService,
        IJwtProvider jwtProvider,
        IMemberRepository memberRepository)
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

                        var member = await memberRepository.GetByUserIdAsync(userId, ct)
                            ?? throw new KeyNotFoundException("Member not found.");

                        var accessToken = await jwtProvider.GenerateAccessTokenAsync(
                            userId,
                            member.Id,
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