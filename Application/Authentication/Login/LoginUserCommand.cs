namespace Application.Authentication.Login
{
    public record LoginUserCommand(
        string Email,
        string Password);
}