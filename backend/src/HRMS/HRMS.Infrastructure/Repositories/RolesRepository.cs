
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Roles;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public RolesRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
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
