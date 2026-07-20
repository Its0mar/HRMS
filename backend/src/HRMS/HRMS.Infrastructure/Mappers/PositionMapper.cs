using HRMS.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class PositionMapper
    {
        public static Position Map(SqlDataReader reader)
        {
            var descriptionAtIndex = reader.GetOrdinal("Description");
            string? description = reader.IsDBNull(descriptionAtIndex)
                ? (string?)null
                : reader.GetString(descriptionAtIndex);

            return Position.Restore(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Title")),
                reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                description
                );
        }
    }
}
