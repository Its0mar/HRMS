using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly ISqlExecutor _executor;

    public UserRepository(ISqlExecutor executor) => _executor = executor;

    public Task<User?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken) =>
        _executor.QueryFirstOrDefaultAsync(
            "dbo.SP_GetUserByIdentifier",
            UserMapper.Map,
            cancellationToken,
            new SqlParameter("@Identifier", identifier));

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _executor.QueryFirstOrDefaultAsync(
            "dbo.Users_GetById",
            UserMapper.Map,
            cancellationToken,
            new SqlParameter("@Id", id));
    }

    public async Task<IReadOnlyList<string>> GetUserPermissions(int userId,  CancellationToken cancellationToken)
    {
        return await _executor.QueryAsync(
            "Permissions_GetForUser",
            map,
            cancellationToken,
            new SqlParameter("@UserId", userId)
            );
    }

    public async Task<bool> UpdateAccessAsync(int employeeId, int organizationId, string username, int roleId, CancellationToken cancellationToken)
    {
        var result = await _executor.ExecuteWithScalarIntAsync(
            "dbo.EmployeeAccess_Update",
            cancellationToken,
            new SqlParameter("@EmployeeId", employeeId),
            new SqlParameter("@OrganizationId", organizationId),
            new SqlParameter("@Username", username),
            new SqlParameter("@RoleId", roleId));

        return result == 1;
    }



    private string map(SqlDataReader reader)
    {
        return reader.GetString(reader.GetOrdinal("Code"));
    }
}
