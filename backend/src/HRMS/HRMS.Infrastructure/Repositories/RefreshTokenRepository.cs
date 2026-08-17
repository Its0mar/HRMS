using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    internal sealed class RefreshTokenRepository(ISqlExecutor sqlExecutor) : IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.RefreshToken_GetByHash",
                RefreshTokenMapper.Map,
                cancellationToken,
                TokenHash("@TokenHash", tokenHash));
        }

        public async Task CreateOrReplaceAsync(
            int userId,
            string tokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            CancellationToken cancellationToken)
        {
            await sqlExecutor.ExecuteAsync(
                "dbo.RefreshToken_CreateOrReplace",
                cancellationToken,
                Int("@UserId", userId),
                TokenHash("@TokenHash", tokenHash),
                DateTime2("@ExpiresAt", expiresAt),
                DateTime2("@CreatedAt", createdAt));
        }

        public async Task<bool> RotateAsync(
            int userId,
            string currentTokenHash,
            string newTokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.RefreshToken_Rotate",
                cancellationToken,
                Int("@UserId", userId),
                TokenHash("@CurrentTokenHash", currentTokenHash),
                TokenHash("@NewTokenHash", newTokenHash),
                DateTime2("@ExpiresAt", expiresAt),
                DateTime2("@CreatedAt", createdAt));

        }

        public async Task<bool> RevokeAsync(string tokenHash, DateTime revokedAt, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.RefreshToken_Revoke",
                cancellationToken,
                TokenHash("@TokenHash", tokenHash),
                DateTime2("@RevokedAt", revokedAt));
        }
    }
}
