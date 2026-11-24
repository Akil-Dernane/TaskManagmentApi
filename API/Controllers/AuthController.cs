using API.Contracts.Authentication;
using API.Contracts.Projects;
using Application.Abstractions.Authentication;
using Application.Authentication.Login;
using Application.Authentication.Logout;
using Application.Authentication.Refresh;
using Application.Authentication.Register;
using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(
        RegisterUserHandler registerUserHandler,
         LoginUserHandler loginUserHandler,
         RefreshTokenHandler refreshTokenHandler,
         LogoutHandler logoutHandler,
         LogoutAllHandler logoutAllHandler) : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResult>> Register(RegisterUserRequest request, CancellationToken ct)
        {
            var result = await registerUserHandler.Handle(
                new RegisterUserCommand(request.Email, request.Password),
                ct);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResult>> Login(LoginUserRequest request, CancellationToken ct)
        {
            var result = await loginUserHandler.Handle(new LoginUserCommand(request.Email, request.Password), ct);

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthenticationResult>> Refresh(RefreshTokenRequest request, CancellationToken ct)
        {
            var result = await refreshTokenHandler.Handle(
                new RefreshTokenCommand(request.RefreshToken),
                ct);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequest request, CancellationToken ct)
        {
            await logoutHandler.Handle(
                new LogoutCommand(request.RefreshToken),
                ct);

            return NoContent();
        }

        [Authorize]
        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out var parsedUserId))
                throw new UnauthorizedException();

            await logoutAllHandler.Handle(
                new LogoutAllCommand(parsedUserId),
                ct);

            return NoContent();
        }
    }
}