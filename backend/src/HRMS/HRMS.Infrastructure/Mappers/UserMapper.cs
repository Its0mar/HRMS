using HRMS.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class UserMapper
    {
        public static User Map(SqlDataReader reader)
        {
            var updatedAtIndex = reader.GetOrdinal("UpdatedAt");
            DateTime? updatedAt = reader.IsDBNull(updatedAtIndex)
                ? (DateTime?)null
                : reader.GetDateTime(updatedAtIndex);

            var employeeIdIndex = reader.GetOrdinal("EmployeeId");
            int? employeeId = reader.IsDBNull(employeeIdIndex)
                ? (int?)null
                : reader.GetInt32(employeeIdIndex);

            return User.Restore(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Username")),
                reader.GetString(reader.GetOrdinal("Email")),
                reader.GetString(reader.GetOrdinal("PasswordHash")),
                reader.GetString(reader.GetOrdinal("FirstName")),
                reader.GetString(reader.GetOrdinal("LastName")),
                reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                updatedAt,
                employeeId
                );
        }
    }
}
