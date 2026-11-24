namespace Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid FamilyId { get; private set; }

        public string TokenHash { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }

        public string? ReplacedByTokenHash { get; private set; }

        private RefreshToken() { }

        public RefreshToken(
            Guid userId,
            Guid familyId,
            string tokenHash,
            DateTime createdAtUtc,
            DateTime expiresAtUtc)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            if (familyId == Guid.Empty)
                throw new ArgumentException("Family ID cannot be empty.", nameof(familyId));

            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Token hash cannot be empty.", nameof(tokenHash));

            EnsureUtc(createdAtUtc, nameof(createdAtUtc));
            EnsureUtc(expiresAtUtc, nameof(expiresAtUtc));

            if (expiresAtUtc <= createdAtUtc)
                throw new ArgumentException(
                    "Expiration must be later than creation time.",
                    nameof(expiresAtUtc));

            Id = Guid.NewGuid();
            UserId = userId;
            FamilyId = familyId;
            TokenHash = tokenHash;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
        }

        public bool IsExpired(DateTime utcNow)
        {
            EnsureUtc(utcNow, nameof(utcNow));

            return utcNow >= ExpiresAtUtc;
        }

        public bool IsRevoked()
        {
            return RevokedAtUtc.HasValue;
        }

        public bool IsActive(DateTime utcNow)
        {
            return !IsRevoked() && !IsExpired(utcNow);
        }

        public void Revoke(
            DateTime revokedAtUtc,
            string? replacedByTokenHash = null)
        {
            EnsureUtc(revokedAtUtc, nameof(revokedAtUtc));

            if (replacedByTokenHash is not null &&
                string.IsNullOrWhiteSpace(replacedByTokenHash))
            {
                throw new ArgumentException(
                    "Replacement token hash cannot be empty.",
                    nameof(replacedByTokenHash));
            }

            if (IsRevoked())
                throw new InvalidOperationException(
                    "Refresh token is already revoked.");

            RevokedAtUtc = revokedAtUtc;
            ReplacedByTokenHash = replacedByTokenHash;
        }

        private static void EnsureUtc(DateTime dateTime, string parameterName)
        {
            if (dateTime.Kind != DateTimeKind.Utc)
                throw new ArgumentException(
                    "DateTime must be UTC.",
                    parameterName);
        }
    }
}