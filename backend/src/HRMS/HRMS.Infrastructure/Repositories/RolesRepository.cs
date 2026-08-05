
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Common;
using HRMS.Domain.Entities.Roles;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public RolesRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public async Task<int> CreateWithPermissionsAsync(Role role, IEnumerable<int> permissionIds, CancellationToken cancellationToken)
        {
            var permissionTable = new DataTable();
            permissionTable.Columns.Add("Id", typeof(int));

            foreach (var permissionId in permissionIds.Distinct())
            {
                permissionTable.Rows.Add(permissionId);
            }

            var permissionParameter = new SqlParameter(
                "@PermissionIds",
                SqlDbType.Structured)
            {
                TypeName = "dbo.IntIdList",
                Value = permissionTable
            };

            return await _sqlExecutor.ExecuteWithScalarIntAsync(
                "Role_Create",
                cancellationToken,
                new SqlParameter("@OrganizationId", role.OrganizationId),
                new SqlParameter("@Name", role.Name),
                permissionParameter
                );
        }

        public async Task<IReadOnlyList<Role>> GetAllAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryAsync(
                "Roles_GetAll",
                RolesMapper.Map,
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId));
        }
    }
}
