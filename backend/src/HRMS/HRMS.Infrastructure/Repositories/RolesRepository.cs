
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Common;
using HRMS.Domain.Entities.Roles;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Mappers.Roles;
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

        public async Task<IReadOnlyList<Role>> GetAllWithPermsAsync(int organizationId, CancellationToken cancellationToken)
        {
            var rows = await _sqlExecutor.QueryAsync(
                "Roles_GetByOrganization",
                RolePermissionRowMapper.MapRolePermissionRow,
                cancellationToken,
                new SqlParameter(
                    "@OrganizationId",
                    SqlDbType.Int)
                {
                    Value = organizationId
                });

                        var roles = rows
                            .GroupBy(row => new
                            {
                                row.RoleId,
                                row.RoleName
                            })
                            .Select(group =>
                            {
                                var permissions = group
                                    .Where(row =>
                                        row.PermissionId.HasValue &&
                                        row.PermissionCode is not null)
                                    .Select(row => new Permission(
                                        row.PermissionId!.Value,
                                        row.PermissionCode!))
                                    .ToList();

                                return Role.Restore(
                                    group.Key.RoleId,
                                    group.Key.RoleName,
                                    organizationId,
                                    permissions);
                            })
                            .ToList();

            return roles;
        }
    }
}
