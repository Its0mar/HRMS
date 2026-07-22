using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public RefreshTokenRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public async Task RemoveForUserAsync(int userId, CancellationToken cancellationToken)
        {
            
            await _sqlExecutor.ExecuteScalarBoolAsync(
                "RefreshToken_Delete",
                cancellationToken,
                new SqlParameter("@UserId", userId)
                );
        }

        public async Task UpdateUserRefreshTokenAsync(int userId, string refreshToken, DateTime expiresAt, DateTime createdAt, CancellationToken cancellationToken)
        {
            await _sqlExecutor.ExecuteScalarBoolAsync(
                "RefreshToken_Update",
                cancellationToken,
                new SqlParameter("@UserId", userId),
                new SqlParameter("@RefreshToken", refreshToken)
                );
        }

        public async Task CreateRefreshTokenAsync(int userId, string refreshToken, DateTime expiresAt, DateTime createdAt, CancellationToken cancellationToken)
        {
            await _sqlExecutor.ExecuteScalarBoolAsync(
                "RefreshToken_Create",
                cancellationToken,
                new SqlParameter("@UserId", userId),
                new SqlParameter("@RefreshToken", refreshToken),
                new SqlParameter("@ExpiresAt", expiresAt),
                new SqlParameter("@CreatedAt", createdAt)
                );
        }

        //public async Task<int> GetUserIdFromRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        //{
        //    return await _sqlExecutor.QueryFirstOrDefaultAsync(
        //        "RefreshToken_GetUser",
        //        UserIdMapper,
        //        cancellationToken,
        //        new SqlParameter("@RefreshToken", refreshToken)
        //        );
        //}

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryFirstOrDefaultAsync(
                "RefreshToken_Get",
                RefreshTokenMapper.Map,
                cancellationToken,
                new SqlParameter("@RefreshToken", refreshToken)
                );
        }
    }
}
