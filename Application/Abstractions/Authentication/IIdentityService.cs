namespace Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<Guid> CreateUserAsync( string email, string password, CancellationToken ct);

        Task<CredentialValidationResult> ValidateCredentialsAsync(string email, string password, CancellationToken ct);

        Task<string?> GetEmailAsync(Guid userId, CancellationToken ct);
    }
}