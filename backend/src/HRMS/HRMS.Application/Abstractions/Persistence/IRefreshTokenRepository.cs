using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IRefreshTokenRepository
    {
        public Task RemoveForUserAsync(int userId, CancellationToken cancellationToken);
        public Task UpdateUserRefreshTokenAsync(int userId, string refreshToken, DateTime expiresAt, DateTime createdAt, CancellationToken cancellationToken);
        public Task CreateRefreshTokenAsync(int userId, string refreshToken, DateTime expiresAt, DateTime createdAt, CancellationToken cancellationToken);
        public Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);


    }
}
