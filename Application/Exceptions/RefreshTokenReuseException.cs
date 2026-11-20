namespace Application.Exceptions
{
    public class RefreshTokenReuseException : ApplicationException
    {
        public RefreshTokenReuseException()
            : base(
                "Refresh token reuse detected. The token family has been revoked.",
                "AUTH_REFRESH_TOKEN_REUSE")
        {
        }
    }
}