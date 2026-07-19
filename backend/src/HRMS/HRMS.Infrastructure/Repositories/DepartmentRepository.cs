using System.Data;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories;

internal sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DepartmentRepository(IDbConnectionFactory connectionFactory) =>
        _connectionFactory = connectionFactory;

    public async Task<int> CreateAsync(Department department, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, "dbo.SP_CreateDepartment");

        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 30) { Value = department.Name });
        command.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar, 6) { Value = department.Code });
        command.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 300)
        {
            Value = string.IsNullOrWhiteSpace(department.Description) ? DBNull.Value : department.Description
        });
        command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.Int) { Value = department.OrganizationId });

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }

    public Task<bool> NameExistsAsync(int organizationId, string name, CancellationToken cancellationToken) =>
        ExistsAsync("dbo.Departments_NameExist", "@Name", name, organizationId, cancellationToken);

    public Task<bool> CodeExistsAsync(int organizationId, string code, CancellationToken cancellationToken) =>
        ExistsAsync("dbo.Departments_CodeExist", "@Code", code, organizationId, cancellationToken);

    private async Task<bool> ExistsAsync(string procedure, string parameterName, string value, int organizationId, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, procedure);
        command.Parameters.Add(new SqlParameter(parameterName, SqlDbType.VarChar) { Value = value });
        command.Parameters.Add(new SqlParameter("@OrganizationId", SqlDbType.Int) { Value = organizationId });
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is not null && result != DBNull.Value && Convert.ToBoolean(result);
    }

    private static SqlCommand CreateCommand(SqlConnection connection, string procedure) => new(procedure, connection)
    {
        CommandType = CommandType.StoredProcedure
    };
}
