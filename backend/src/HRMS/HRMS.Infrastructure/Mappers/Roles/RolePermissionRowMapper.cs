using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Infrastructure.Mappers.Roles
{
    public static class RolePermissionRowMapper
    {
        public static RolePermissionRow MapRolePermissionRow(SqlDataReader reader)
        {
            var permissionIdOrdinal =
                reader.GetOrdinal("PermissionId");

            var permissionCodeOrdinal =
                reader.GetOrdinal("PermissionCode");

            return new RolePermissionRow(
                reader.GetInt32(reader.GetOrdinal("RoleId")),
                reader.GetString(reader.GetOrdinal("RoleName")),

                reader.IsDBNull(permissionIdOrdinal)
                    ? null
                    : reader.GetInt32(permissionIdOrdinal),

                reader.IsDBNull(permissionCodeOrdinal)
                    ? null
                    : reader.GetString(permissionCodeOrdinal));
        }
    }
}
