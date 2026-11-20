namespace Application.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        public string ErrorCode { get; }

        protected ApplicationException(
            string message,
            string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}