using HRMS.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class RefreshTokenMapper
    {
        public static RefreshToken Map(SqlDataReader reader)
        {
            var revokedAtIndex = reader.GetOrdinal("RevokedAt");
            DateTime? revokedAt = reader.IsDBNull(revokedAtIndex)
                ? (DateTime?)null
                : reader.GetDateTime(revokedAtIndex);

            return RefreshToken.Restore(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("UserId")),
                reader.GetString(reader.GetOrdinal("Token")),
                reader.GetDateTime(reader.GetOrdinal("ExpiresAt")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                revokedAt
                );
        }
    }
}
