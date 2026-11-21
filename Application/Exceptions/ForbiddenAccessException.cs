namespace Application.Exceptions
{
    public class ForbiddenAccessException : ApplicationException
    {
        public ForbiddenAccessException()
            : base("You don't have permission to access this resource.", "FORBIDDEN_ACCESS")
        {
        }
    }
}