using Application.Abstractions.Authentication;

namespace Application.Authentication.Logout
{
    public class LogoutHandler(
        IRefreshTokenService refreshTokenService)
    {
        public Task Handle(
            LogoutCommand command,
            CancellationToken ct)
        {
            return refreshTokenService.RevokeFamilyAsync(
                command.RefreshToken,
                ct);
        }
    }
}