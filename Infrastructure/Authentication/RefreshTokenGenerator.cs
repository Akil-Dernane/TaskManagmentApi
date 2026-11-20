using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return WebEncoders.Base64UrlEncode(randomBytes);
        }

        public string Hash(string token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = SHA256.HashData(bytes);

            return Convert.ToHexString(hash);
        }
    }
}