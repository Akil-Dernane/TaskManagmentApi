using Application.Abstractions;
using Application.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IClock _clock;
        public JwtProvider(IOptions<JwtSettings> options, IClock clock)
        {
            _jwtSettings = options.Value;
            _clock = clock;
        }
        public Task<AccessTokenResult> GenerateAccessTokenAsync(Guid userId, string email, CancellationToken ct)
        {
            var expiresAtUtc = _clock.UtcNow.AddMinutes(
                _jwtSettings.AccessTokenMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = expiresAtUtc,
                SigningCredentials = new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256)
            };

            var token = new JsonWebTokenHandler().CreateToken(descriptor);

            return Task.FromResult(
                new AccessTokenResult(token, expiresAtUtc));
        }
    }
}
