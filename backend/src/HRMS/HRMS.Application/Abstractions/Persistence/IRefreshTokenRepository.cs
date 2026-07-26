using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);

        Task CreateOrReplaceAsync(
            int userId,
            string tokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            CancellationToken cancellationToken);

        Task<bool> RotateAsync(
            int userId,
            string currentTokenHash,
            string newTokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            CancellationToken cancellationToken);

        Task<bool> RevokeAsync(
            string tokenHash,
            DateTime revokedAt,
            CancellationToken cancellationToken);


    }
}
