
namespace HRMS.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }


        public RefreshToken(int id, int userId, string token, DateTime expiresAt, DateTime? revokedAt, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            RevokedAt = revokedAt;
            CreatedAt = createdAt;
        }
    }
}
