using System.Data;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories;

internal sealed class RegistrationRepository(
    IDbConnectionFactory connectionFactory,
    ISqlExecutor sqlExecutor) : IRegistrationRepository
{
    public Task<bool> OrganizationCodeExistsAsync(string code, CancellationToken cancellationToken) =>
        sqlExecutor.ExecuteScalarBoolAsync("dbo.Organization_CodeExists", cancellationToken, VarChar("@Code", 10, code));
    public Task<bool> OrganizationEmailExistsAsync(string email, CancellationToken cancellationToken) =>
        sqlExecutor.ExecuteScalarBoolAsync("dbo.Organization_EmailExists", cancellationToken, VarChar("@Email", 40, email));
    public Task<bool> UserEmailExistsAsync(string email, CancellationToken cancellationToken) =>
        sqlExecutor.ExecuteScalarBoolAsync("dbo.User_EmailExists", cancellationToken, VarChar("@Email", 40, email));
    public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken) =>
        sqlExecutor.ExecuteScalarBoolAsync("dbo.User_UsernameExists", cancellationToken, VarChar("@Username", 20, username));
    public async Task<OrganizationRegistrationResult> RegisterOrganizationWithUserAsync(
        Organization organization,
        User user,
        CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var organizationId = await CreateOrganizationAsync(connection, transaction, organization, cancellationToken);

            var ownerUserId = await CreateUserAsync(connection, transaction, user, organizationId, cancellationToken);
            await AssignRoleAsync(connection, transaction, ownerUserId, roleId: 10, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new OrganizationRegistrationResult(organizationId, ownerUserId);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
    public async Task<int> UserRegisterAsync(User user, int roleId, CancellationToken cancellationToken)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var userId = await CreateUserAsync(connection, transaction, user, null, cancellationToken);
            await AssignRoleAsync(connection, transaction, userId, roleId, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return userId;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
    private static async Task<int> CreateOrganizationAsync(SqlConnection connection, SqlTransaction transaction, Organization organization, CancellationToken cancellationToken)
    {
        await using var command = CreateCommand(connection, transaction, "dbo.Organization_Create");
        command.Parameters.AddRange([
            VarChar("@Name", 30, organization.Name),
            VarChar("@Code", 10, organization.Code),
            VarChar("@Email", 40, organization.Email),
            NullableVarChar("@Address", 100, organization.Address),
            NullableVarChar("@Website", 100, organization.Website),
            NullableVarChar("@LogoUrl", 100, organization.LogoUrl),
            Bit("@IsActive", organization.IsActive),
            Bit("@IsDeleted", organization.IsDeleted),
            DateTime2("@CreatedAt", organization.CreatedAt)
        ]);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }
    private static async Task<int> CreateUserAsync(SqlConnection connection, SqlTransaction transaction, User user, int? organizationId,CancellationToken cancellationToken)
    {
        await using var command = CreateCommand(connection, transaction, "dbo.User_Create");
        command.Parameters.AddRange([
            Int("@OrganizationId", organizationId ?? user.OrganizationId),
            VarChar("@Username", 20, user.Username),
            VarChar("@Email", 40, user.Email),
            VarChar("@PasswordHash", -1, user.PasswordHash),
            VarChar("@FirstName", 20, user.FirstName),
            VarChar("@LastName", 20, user.LastName),
            Bit("@IsActive", true),
            Bit("@IsDeleted", false),
            DateTime2("@CreatedAt", DateTime.UtcNow),
            NullableInt("@EmployeeId", user.EmployeeId)
        ]);
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }
    private static async Task AssignRoleAsync(SqlConnection connection, SqlTransaction transaction, int userId, int roleId, CancellationToken cancellationToken)
    {
        await using var command = CreateCommand(connection, transaction, "dbo.UserRole_Create");
        command.Parameters.AddRange([
            Int("@UserId", userId),
            Int("@RoleId", roleId),
            DateTime2("@CreatedAt", DateTime.UtcNow)
        ]);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
    private static SqlCommand CreateCommand(SqlConnection connection, SqlTransaction transaction, string procedure) => new(procedure, connection, transaction)
    {
        CommandType = CommandType.StoredProcedure
    };
}
