
using HRMS.Domain.Entities.Roles;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class RolesMapper
    {
        public static Role Map(SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var name = reader.GetString(reader.GetOrdinal("Name"));
            var organization = reader.GetInt32(reader.GetOrdinal("OrganizationId"));

            return Role.Restore(id, name, organization);
        }
    }
}
