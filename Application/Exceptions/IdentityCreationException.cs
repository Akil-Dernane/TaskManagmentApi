namespace Application.Exceptions
{
    public class IdentityCreationException : ApplicationException
    {
        public IReadOnlyCollection<string> Errors { get; }

        public IdentityCreationException(IEnumerable<string> errors)
            : base("User creation failed.", "IDENTITY_USER_CREATION_FAILED")
        {
            Errors = errors.ToList();
        }
    }
}