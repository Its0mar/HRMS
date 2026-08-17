using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories;

internal sealed class UserRepository(ISqlExecutor sqlExecutor) : IUserRepository
{
    public async Task<User?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken) =>
        await sqlExecutor.QueryFirstOrDefaultAsync(
            "dbo.SP_GetUserByIdentifier",
            UserMapper.Map,
            cancellationToken,
            VarChar("@Identifier",40, identifier));

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
         await sqlExecutor.QueryFirstOrDefaultAsync(
            "dbo.Users_GetById",
            UserMapper.Map,
            cancellationToken,
            Int("@Id", id));
    

    public async Task<IReadOnlyList<string>> GetUserPermissions(int userId,  CancellationToken cancellationToken) =>
        await sqlExecutor.QueryAsync(
            "Permissions_GetForUser",
            reader => reader.GetString(reader.GetOrdinal("Code")),
            cancellationToken,
            Int("@UserId", userId));

    public async Task<bool> UpdateAccessAsync(int employeeId, int organizationId, string username, int roleId, CancellationToken cancellationToken)
    {
        var result = await sqlExecutor.ExecuteWithScalarIntAsync(
            "dbo.EmployeeAccess_Update",
            cancellationToken,
            Int("@EmployeeId", employeeId),
            Int("@OrganizationId", organizationId),
            VarChar("@Username",40, username),
            Int("@RoleId", roleId));

        return result == 1;
    }
}
