using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication
{
    public class JwtSettings
    {
        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Required]
        [MinLength(32)]
        public string SecretKey { get; set; } = string.Empty;

        [Range(1, 60)]
        public int AccessTokenMinutes { get; set; } = 15;

        [Range(1, 365)]
        public int RefreshTokenDays { get; set; } = 30;
    }
}