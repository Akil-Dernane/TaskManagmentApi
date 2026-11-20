
using Application.Abstractions;
using Application.Abstractions.Authentication;
using Application.Abstractions.Persistence;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IRefreshTokenGenerator _generator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(
            IRefreshTokenRepository repository,
            IRefreshTokenGenerator generator,
            IUnitOfWork unitOfWork,
            IClock clock,
            IOptions<JwtSettings> options)
        {
            _repository = repository;
            _generator = generator;
            _unitOfWork = unitOfWork;
            _clock = clock;
            _jwtSettings = options.Value;
        }

        public async Task<RefreshTokenResult> CreateAsync(Guid userId, Guid? familyId, CancellationToken ct)
        {
            var now = _clock.UtcNow;
            var expiresAtUtc = now.AddDays(_jwtSettings.RefreshTokenDays);

            var rawToken = _generator.Generate();
            var tokenHash = _generator.Hash(rawToken);

            var refreshToken = new RefreshToken(
                userId,
                familyId ?? Guid.NewGuid(),
                tokenHash,
                now,
                expiresAtUtc);

            _repository.Add(refreshToken);

            await _unitOfWork.SaveChangesAsync(ct);

            return new RefreshTokenResult(rawToken, expiresAtUtc);
        }

        public async Task RevokeFamilyAsync(string token, CancellationToken ct)
        {
            var existingToken = await GetTokenAsync(token, ct);

            await RevokeFamilyTokensAsync(
                existingToken.FamilyId,
                _clock.UtcNow,
                ct);
        }

        public async Task RevokeAllAsync(Guid userId, CancellationToken ct)
        {
            var userTokens = await _repository.GetByUserIdAsync(
                userId,
                ct);

            var now = _clock.UtcNow;

            foreach (var refreshToken in userTokens)
            {
                refreshToken.Revoke(now);
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<RefreshTokenRotationResult> RotateAsync(string token, CancellationToken ct)
        {
            var existingToken = await GetTokenAsync(token, ct);
            var now = _clock.UtcNow;

            // Detect reuse of a previously revoked token.
            if (existingToken.IsRevoked())
            {
                await RevokeFamilyTokensAsync(
                    existingToken.FamilyId,
                    now,
                    ct);

                throw new RefreshTokenReuseException();
            }

            if (existingToken.IsExpired(now))
                throw new InvalidRefreshTokenException();

            var newRawToken = _generator.Generate();
            var newTokenHash = _generator.Hash(newRawToken);

            existingToken.Revoke(now, newTokenHash);

            var expiresAtUtc = now.AddDays(_jwtSettings.RefreshTokenDays);

            var newRefreshToken = new RefreshToken(
                existingToken.UserId,
                existingToken.FamilyId,
                newTokenHash,
                now,
                expiresAtUtc);

            _repository.Add(newRefreshToken);

            try
            {
                await _unitOfWork.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new InvalidRefreshTokenException();
            }

            return new RefreshTokenRotationResult(
                existingToken.UserId,
                new RefreshTokenResult(newRawToken, expiresAtUtc));
        }

        private async Task<RefreshToken> GetTokenAsync(string token, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidRefreshTokenException();

            var tokenHash = _generator.Hash(token);

            var existingToken = await _repository.GetByTokenHashAsync(
                tokenHash,
                ct);

            return existingToken
                ?? throw new InvalidRefreshTokenException();
        }

        private async Task RevokeFamilyTokensAsync(Guid familyId, DateTime now, CancellationToken ct)
        {
            var familyTokens = await _repository.GetByFamilyIdAsync(
                familyId,
                ct);

            foreach (var familyToken in familyTokens)
            {
                if (!familyToken.IsRevoked())
                    familyToken.Revoke(now);
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
