using Application.Abstractions.Authentication;
using Application.Exceptions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Guid> CreateUserAsync(string email, string password, CancellationToken ct)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = email
            };

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);

                throw new IdentityCreationException(errors);
            }

            return user.Id;
        }

        public async Task<string?> GetEmailAsync(
            Guid userId,
            CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return user?.Email;
        }

        public async Task<CredentialValidationResult> ValidateCredentialsAsync(string email, string password, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new CredentialValidationResult(
                    null,
                    CredentialValidationStatus.InvalidCredentials);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (result.IsLockedOut)
            {
                return new CredentialValidationResult(
                    null,
                    CredentialValidationStatus.LockedOut);
            }

            if (result.IsNotAllowed)
            {
                return new CredentialValidationResult(
                    null,
                    CredentialValidationStatus.NotAllowed);
            }

            if (!result.Succeeded)
            {
                return new CredentialValidationResult(
                    null,
                    CredentialValidationStatus.InvalidCredentials);
            }

            return new CredentialValidationResult(
                user.Id,
                CredentialValidationStatus.Success);
        }
    }
}