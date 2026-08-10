using System.Data;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories;

internal sealed class RegistrationRepository : IRegistrationRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RegistrationRepository(IDbConnectionFactory connectionFactory) =>
        _connectionFactory = connectionFactory;

    public Task<bool> OrganizationCodeExistsAsync(string code, CancellationToken cancellationToken) =>
        ExistsAsync("dbo.Organization_CodeExists", "@Code", SqlDbType.VarChar, 10, code, cancellationToken);

    public Task<bool> OrganizationEmailExistsAsync(string email, CancellationToken cancellationToken) =>
        ExistsAsync("dbo.Organization_EmailExists", "@Email", SqlDbType.VarChar, 40, email, cancellationToken);

    public Task<bool> UserEmailExistsAsync(string email, CancellationToken cancellationToken) =>
        ExistsAsync("dbo.User_EmailExists", "@Email", SqlDbType.VarChar, 40, email, cancellationToken);

    public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken) =>
        ExistsAsync("dbo.User_UsernameExists", "@Username", SqlDbType.VarChar, 20, username, cancellationToken);

    public async Task<OrganizationRegistrationResult> RegisterOrganizationWithUserAsync(
        Organization organization,
        User user,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var organizationId = await CreateOrganizationAsync(connection, transaction, organization, cancellationToken);
            var ownerUserId = await CreateUserAsync(connection, transaction, user, cancellationToken);
            //var roleId = await CreateRoleAsync(connection, transaction, organizationId, "OrganizationOwner", cancellationToken);
            await AssignRoleAsync(connection, transaction, ownerUserId, 10, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new OrganizationRegistrationResult(organizationId, ownerUserId);
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    public async Task<int> UserRegisterAsync(
        User user,
        int roleId,
        CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var userId = await CreateUserAsync(
                connection,
                transaction,
                user,
                cancellationToken);

            await AssignRoleAsync(
                connection,
                transaction,
                userId,
                roleId,
                cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return userId;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    private async Task<bool> ExistsAsync(string procedure, string name, SqlDbType type, int size, string value, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, null, procedure);
        command.Parameters.Add(new SqlParameter(name, type, size) { Value = value });
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is not null && result != DBNull.Value && Convert.ToBoolean(result);
    }

    private static async Task<int> CreateOrganizationAsync(SqlConnection connection, SqlTransaction? transaction, Organization organization, CancellationToken cancellationToken)
    {
        await using var command = CreateCommand(connection, transaction, "dbo.Organization_Create");
        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 30) { Value = organization.Name });
        command.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar, 10) { Value = organization.Code });
        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 40) { Value = organization.Email });
        command.Parameters.Add(NullableVarchar("@Address", 100, organization.Address));
        command.Parameters.Add(NullableVarchar("@Website", 100, organization.Website));
        command.Parameters.Add(NullableVarchar("@LogoUrl", 100, organization.LogoUrl));
        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = organization.IsActive });
        command.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = organization.IsDeleted });
        command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime2) { Value = organization.CreatedAt });
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    private static async Task<int> CreateUserAsync(SqlConnection connection, SqlTransaction transaction, User user, CancellationToken cancellationToken)
    {
        await using var command = CreateCommand(connection, transaction, "dbo.User_Create");
        command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.Int) { Value = user.OrganizationId });
        command.Parameters.Add(new SqlParameter("@Username", SqlDbType.VarChar, 20) { Value = user.Username });
        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 40) { Value = user.Email });
        command.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.VarChar, -1) { Value = user.PasswordHash });
        command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar, 20) { Value = user.FirstName });
        command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar, 20) { Value = user.LastName });
        command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = true });
        command.Parameters.Add(new SqlParameter("@IsDeleted", SqlDbType.Bit) { Value = false });
        command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime2) { Value = DateTime.UtcNow });
        command.Parameters.Add(new SqlParameter("@EmployeeId", user.EmployeeId ?? (object)DBNull.Value));
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
    }

    private static async Task AssignRoleAsync(SqlConnection connection, SqlTransaction transaction, int userId, int roleId, CancellationToken cancellationToken)
    {
        await using var command = CreateCommand(connection, transaction, "dbo.UserRole_Create");
        command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
        command.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.Int) { Value = roleId });
        command.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime2) { Value = DateTime.UtcNow });
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static SqlCommand CreateCommand(SqlConnection connection, SqlTransaction? transaction, string procedure) => new(procedure, connection, transaction)
    {
        CommandType = CommandType.StoredProcedure
    };

    private static SqlParameter NullableVarchar(string name, int size, string? value) => new(name, SqlDbType.VarChar, size)
    {
        Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value
    };
}
