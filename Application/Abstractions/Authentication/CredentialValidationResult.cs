namespace Application.Abstractions.Authentication
{
    public record CredentialValidationResult(
        Guid? UserId,
        CredentialValidationStatus Status);
}