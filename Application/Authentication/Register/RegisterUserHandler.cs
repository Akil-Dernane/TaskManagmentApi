using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using Domain.Entities;

namespace Application.Authentication.Register
{
    public class RegisterUserHandler(
        IIdentityService identityService,
        IJwtProvider jwtProvider,
        IRefreshTokenService refreshTokenService,
        IMemberRepository memberRepository,
        IUnitOfWork unitOfWork)
    {
        public async Task<AuthenticationResult> Handle(
            RegisterUserCommand command,
            CancellationToken ct)
        {
            var userId = await identityService.CreateUserAsync(
                command.Email,
                command.Password,
                ct);

            var member = new Member(command.Email);

            member.LinkToUser(userId);

            await memberRepository.AddAsync(member, ct);

            await unitOfWork.SaveChangesAsync(ct);

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
    }
}