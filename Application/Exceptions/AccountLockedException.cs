namespace Application.Exceptions
{
    public class AccountLockedException : ApplicationException
    {
        public AccountLockedException()
            : base("Your account is temporarily locked.", "AUTH_ACCOUNT_LOCKED")
        {
        }
    }
}