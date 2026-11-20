namespace Application.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException()
            : base(
                "Invalid email or password.",
                "AUTH_INVALID_CREDENTIALS")
        {
        }
    }
}