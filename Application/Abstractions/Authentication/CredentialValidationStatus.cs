namespace Application.Abstractions.Authentication
{
    public enum CredentialValidationStatus
    {
        Success,
        InvalidCredentials,
        LockedOut,
        NotAllowed
    }
}