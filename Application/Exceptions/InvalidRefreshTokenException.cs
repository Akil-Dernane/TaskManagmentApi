namespace Application.Exceptions
{
    public class InvalidRefreshTokenException : ApplicationException
    {
        public InvalidRefreshTokenException()
            : base(
                "The refresh token is invalid or expired.",
                "AUTH_INVALID_REFRESH_TOKEN")
        {
        }
    }
}