using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Roles;
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
            var permissionParameter = CreatePermissionIdsParameter(permissionIds);

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

        public async Task<Role?> GetByIdAsync(int id, int organizationId, CancellationToken cancellationToken)
        {
            var rows = await _sqlExecutor.QueryAsync(
                "dbo.Roles_GetById",
                RolePermissionRowMapper.MapRolePermissionRow,
                cancellationToken,
                new SqlParameter("@Id", id),
                new SqlParameter("@OrganizationId", organizationId));

            if (rows.Count == 0)
            {
                return null;
            }

            var firstRow = rows[0];

            var permissions = rows
                .Where(row =>
                    row.PermissionId.HasValue &&
                    row.PermissionCode is not null)
                .Select(row => new Permission(
                    row.PermissionId!.Value,
                    row.PermissionCode!))
                .DistinctBy(permission => permission.Id)
                .ToList();

            return Role.Restore(
                firstRow.RoleId,
                firstRow.RoleName,
                organizationId,
                permissions);
        }

        public async Task<int> UpdateWithPermissionsAsync(Role role, IEnumerable<int> permissionIds, CancellationToken cancellationToken)
        {
            if (!role.Id.HasValue)
            {
                throw new InvalidOperationException("A role must have an ID before it can be updated.");
            }

            var permissionParameter =
                CreatePermissionIdsParameter(permissionIds);

            return await _sqlExecutor.ExecuteWithScalarIntAsync(
                "dbo.Role_Update",
                cancellationToken,
                new SqlParameter("@Id", role.Id.Value),
                new SqlParameter("@OrganizationId", role.OrganizationId),
                new SqlParameter("@Name", role.Name),
                permissionParameter);
        }


        private static SqlParameter CreatePermissionIdsParameter(IEnumerable<int> permissionIds)
        {
            var permissionTable = new DataTable();

            permissionTable.Columns.Add("Id", typeof(int));

            foreach (var permissionId in permissionIds.Distinct())
            {
                permissionTable.Rows.Add(permissionId);
            }

            return new SqlParameter(
                "@PermissionIds",
                SqlDbType.Structured)
            {
                TypeName = "dbo.IntIdList",
                Value = permissionTable
            };
        }
    }
}
