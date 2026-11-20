namespace Application.Exceptions
{
    public class ForbiddenException : ApplicationException
    {
        public ForbiddenException()
            : base("Sign-in is not currently allowed.", "AUTH_SIGN_IN_NOT_ALLOWED")
        {
        }
    }
}