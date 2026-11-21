using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using Application.Exceptions;

namespace Application.Authentication.Refresh
{
    public class RefreshTokenHandler(
        IRefreshTokenService refreshTokenService,
        IJwtProvider jwtProvider,
        IIdentityService identityService,
        IMemberRepository memberRepository)
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

            var member = await memberRepository.GetByUserIdAsync(
                rotationResult.UserId,
                ct) ?? throw new KeyNotFoundException("Member not found.");

            var accessToken = await jwtProvider.GenerateAccessTokenAsync(
                rotationResult.UserId,
                member.Id,
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