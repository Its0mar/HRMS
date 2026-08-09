using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;


namespace HRMS.Infrastructure.Repositories
{
    public sealed class PermissionsRepository : IPermissionsRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public PermissionsRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public async Task<IReadOnlyList<PermissionOption>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryAsync(
                "dbo.Permissions_GetAll",
                Map,
                cancellationToken);
        }

        private static PermissionOption Map(SqlDataReader reader)
        {
            return new PermissionOption(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("Code")),
                reader.GetString(reader.GetOrdinal("Description")));
        }
    }
}
