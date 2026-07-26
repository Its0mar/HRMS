using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Repositories
{
    internal sealed class RefreshTokenRepository
    : IRefreshTokenRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public RefreshTokenRepository(
            ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public Task<RefreshToken?> GetByHashAsync(
            string tokenHash,
            CancellationToken cancellationToken)
        {
            return _sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.RefreshToken_GetByHash",
                RefreshTokenMapper.Map,
                cancellationToken,
                TokenHashParameter(
                    "@TokenHash",
                    tokenHash));
        }

        public async Task CreateOrReplaceAsync(
            int userId,
            string tokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            CancellationToken cancellationToken)
        {
            await _sqlExecutor.ExecuteAsync(
                "dbo.RefreshToken_CreateOrReplace",
                cancellationToken,

                new SqlParameter(
                    "@UserId",
                    SqlDbType.Int)
                {
                    Value = userId
                },

                TokenHashParameter(
                    "@TokenHash",
                    tokenHash),

                new SqlParameter(
                    "@ExpiresAt",
                    SqlDbType.DateTime2)
                {
                    Value = expiresAt
                },

                new SqlParameter(
                    "@CreatedAt",
                    SqlDbType.DateTime2)
                {
                    Value = createdAt
                });
        }

        public Task<bool> RotateAsync(
            int userId,
            string currentTokenHash,
            string newTokenHash,
            DateTime expiresAt,
            DateTime createdAt,
            CancellationToken cancellationToken)
        {
            return _sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.RefreshToken_Rotate",
                cancellationToken,

                new SqlParameter(
                    "@UserId",
                    SqlDbType.Int)
                {
                    Value = userId
                },

                TokenHashParameter(
                    "@CurrentTokenHash",
                    currentTokenHash),

                TokenHashParameter(
                    "@NewTokenHash",
                    newTokenHash),

                new SqlParameter(
                    "@ExpiresAt",
                    SqlDbType.DateTime2)
                {
                    Value = expiresAt
                },

                new SqlParameter(
                    "@CreatedAt",
                    SqlDbType.DateTime2)
                {
                    Value = createdAt
                });
        }

        public Task<bool> RevokeAsync(
            string tokenHash,
            DateTime revokedAt,
            CancellationToken cancellationToken)
        {
            return _sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.RefreshToken_Revoke",
                cancellationToken,

                TokenHashParameter(
                    "@TokenHash",
                    tokenHash),

                new SqlParameter(
                    "@RevokedAt",
                    SqlDbType.DateTime2)
                {
                    Value = revokedAt
                });
        }

        private static SqlParameter TokenHashParameter(
            string name,
            string tokenHash)
        {
            return new SqlParameter(
                name,
                SqlDbType.Char,
                64)
            {
                Value = tokenHash
            };
        }
    }
}
