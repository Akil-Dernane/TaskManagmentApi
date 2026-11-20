using Application.Abstractions.Authentication;

namespace Application.Authentication.Logout
{
    public class LogoutAllHandler(
        IRefreshTokenService refreshTokenService)
    {
        public Task Handle(
            LogoutAllCommand command,
            CancellationToken ct)
        {
            return refreshTokenService.RevokeAllAsync(
                command.UserId,
                ct);
        }
    }
}