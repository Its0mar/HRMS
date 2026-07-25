using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories
{
    internal class PositionsRepository : IPositionsRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public PositionsRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public async Task<int> CreateAsync(Position position, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.ExecuteWithScalarIntAsync(
                "Positions_Create",
                cancellationToken,
                new SqlParameter("@Title", position.Title),
                new SqlParameter("@Description", position.Description),
                new SqlParameter("@OrganizationId", position.OrganizationId)
                );
        }

        public Task<Department?> GettByIdAsync(int id, int organizationId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> TitleExistsAsync(int organizationId, string title, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.ExecuteScalarBoolAsync(
                "Positions_TitleExist",
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId),
                new SqlParameter("@Title", title)
                );
        }

        public async Task<List<Position>> GetPositionsAsync(int organizationId, CancellationToken cancellationToken)
        {
            var positions = await  _sqlExecutor.QueryAsync(
                "Positions_GetAll",
                PositionMapper.Map,
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId));

            return positions ?? new List<Position>();
        }
    }
}
