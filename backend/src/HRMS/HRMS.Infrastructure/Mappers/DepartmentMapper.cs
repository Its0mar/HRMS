using HRMS.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class DepartmentMapper
    {
        public static Department Map(SqlDataReader reader)
        {
            var descriptionAtIndex= reader.GetOrdinal("Description");
            string? description = reader.IsDBNull(descriptionAtIndex)
                ? (string?)null
                : reader.GetString(descriptionAtIndex);

            var managerIdAtIndex = reader.GetOrdinal("ManagerEmployeeId");
            int? managerEmployeeId = reader.IsDBNull(managerIdAtIndex)
                ? (int?)null
                : reader.GetInt32(managerIdAtIndex);

            var updatedAtIndex = reader.GetOrdinal("UpdatedAt");
            DateTime? updatedAt = reader.IsDBNull(updatedAtIndex)
                ? (DateTime?)null
                : reader.GetDateTime(updatedAtIndex);

            return Department.Restore(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Code")),
                reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                reader.GetBoolean(reader.GetOrdinal("IsActive")),
                reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                description,
                managerEmployeeId,
                updatedAt);
        }
    }
}