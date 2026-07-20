using System.Data;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Repositories;

internal sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ISqlExecutor _sqlExecutor;

    public DepartmentRepository(IDbConnectionFactory connectionFactory, ISqlExecutor sqlExecutor)
    {
        _connectionFactory = connectionFactory;
        _sqlExecutor = sqlExecutor;
    }
        


    public async Task<int> CreateAsync(Department department, CancellationToken cancellationToken)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        await using var command = CreateCommand(connection, "dbo.SP_CreateDepartment");

        command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 30) { Value = department.Name });
        command.Parameters.Add(new SqlParameter("@Code", SqlDbType.VarChar, 6) { Value = department.Code });
        command.Parameters.Add(new SqlParameter("@ManagerEmployeeId", SqlDbType.Int) { Value = department.ManagerEmployeeId });
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

    public async Task<bool> UpdateDepartmentAsync(int departmentId, Department department, CancellationToken cancellationToken)
    {
        var intResult =  await _sqlExecutor.ExecuteAsync("dbo.Departments_Update",
            cancellationToken,
            new SqlParameter("@Id", department.Id),
            new SqlParameter("@Name", department.Name),
            new SqlParameter("@Description", department.Description),
            new SqlParameter("@ManagerEmployeeId", department.ManagerEmployeeId),
            new SqlParameter("@OrganizationId", department.OrganizationId));

        return intResult > 0;
    }

    public async Task<Department?> GettByIdAsync(int id, int organizationId, CancellationToken ct)
    {
        var department = await _sqlExecutor.QueryFirstOrDefaultAsync(
            "dbo.Departments_GetById",
            DepartmentMapper.Map,
            ct,
            new SqlParameter("@Id", id),
            new SqlParameter("@OrganizationId", organizationId));

        return department;
    }

    public async Task<List<Department>> GetDepartmentsAsync(int organizationId, CancellationToken cancellationToken)
    {
        var departments = await _sqlExecutor.QueryAsync(
            "Departments_GetAll",
            DepartmentMapper.Map,
            cancellationToken,
            new SqlParameter("@OrganizationId", organizationId));

        return departments ?? new List<Department>();
    }
}
