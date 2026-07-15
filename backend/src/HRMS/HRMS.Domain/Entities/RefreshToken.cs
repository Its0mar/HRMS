
using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities
{
    public class RefreshToken
    {
        public int? Id { get; private set; }
        public int UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsValid => !IsExpired && !IsRevoked;


        public RefreshToken(
            int userId,
            string token,
            DateTime expiresAt)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public static RefreshToken Restore(
            int id,
            int userId,
            string token,
            DateTime expiresAt,
            DateTime createdAt,
            DateTime? revokedAt)
        {
            var refreshToken = new RefreshToken(userId, token, expiresAt)
            {
                Id = id,
                CreatedAt = createdAt,
                RevokedAt = revokedAt
            };

            return refreshToken;

        }

        public void Revoke()
        {
            if (IsRevoked) return;

            RevokedAt = DateTime.UtcNow;
        }
    }
}
