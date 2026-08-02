using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Employees.GetEmployeeOptions;
using HRMS.Application.Features.Employees.GetEmployees;
using HRMS.Domain.Entities.Employees;
using HRMS.Infrastructure.Mappers;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlExecutor _sqlExecutor;

        public EmployeeRepository(
            ISqlExecutor sqlExecutor
            )
        {
            _sqlExecutor = sqlExecutor;
        }

        public async Task<int> CreateAsync(Employee employee, CancellationToken cancellationToken)
        {
            var parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@OrganizationId", employee.OrganizationId));
            parms.Add(new SqlParameter("@EmployeeNumber", employee.EmployeeNumber));

            parms.Add(new SqlParameter("@FirstName", employee.PersonalInformation.FirstName));
            parms.Add(new SqlParameter("@LastName", employee.PersonalInformation.LastName));
            parms.Add(new SqlParameter("@DateOfBirth", employee.PersonalInformation.DateOfBirth.ToDateTime(TimeOnly.MinValue)));
            parms.Add(new SqlParameter("@GenderId", (byte)employee.PersonalInformation.Gender));
            parms.Add(new SqlParameter("@NationalId", employee.PersonalInformation.NationalId));
            parms.Add(new SqlParameter("@Nationality", employee.PersonalInformation.Nationality));
            parms.Add(new SqlParameter("@MaritalStatusId",(byte) employee.PersonalInformation.MaritalStatus));
            parms.Add(new SqlParameter("@Phone", employee.PersonalInformation.Phone));
            parms.Add(new SqlParameter("@Email", employee.PersonalInformation.Email));
            parms.Add(new SqlParameter("@Address", employee.PersonalInformation.Address));
            parms.Add(NullableVarchar("@ProfilePictureUrl", 300, employee.PersonalInformation.ProfilePictureUrl));

            parms.Add(new SqlParameter("@DepartmentId", employee.EmploymentInformation.DepartmentId));
            parms.Add(new SqlParameter("@PositionId", employee.EmploymentInformation.PositionId));
            parms.Add(NullableInt("@ManagerEmployeeId", employee.EmploymentInformation.ManagerEmployeeId));
            parms.Add(new SqlParameter("@HireDate", employee.EmploymentInformation.HireDate.ToDateTime(TimeOnly.MinValue)));
            parms.Add(new SqlParameter("@EmploymentTypeId", (byte) employee.EmploymentInformation.EmploymentType));
            parms.Add(new SqlParameter("@EmploymentStatusId", (byte)employee.EmploymentInformation.EmploymentStatus));
            parms.Add(new SqlParameter("@WorkEmail", employee.EmploymentInformation.WorkEmail));
            parms.Add(NullableVarchar("@WorkPhone", 30, employee.EmploymentInformation.WorkPhone));



            return await _sqlExecutor.ExecuteWithScalarIntAsync(
                "dbo.Employee_Create",
                cancellationToken, 
                parms.ToArray());
        }

        public async Task<List<GetEmployeesResponse>> GetEmployeesAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryAsync(
                "Employees_GetAll",
                GetEmployeesResponseMapper.Map,
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId)
                );
        }
        public async Task<List<EmployeeOptionResponse>> GetEmployeesOptionsAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await _sqlExecutor.QueryAsync(
                "Employees_GetOptions",
                Map,
                cancellationToken,
                new SqlParameter("@OrganizationId", organizationId)
                );
        }

        private EmployeeOptionResponse Map(SqlDataReader reader)
        {
            var id = reader.GetInt32(reader.GetOrdinal("Id"));
            var empNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber"));
            var name = reader.GetString(reader.GetOrdinal("FullName"));

            return new EmployeeOptionResponse(id,empNumber , name);
        }
        private static SqlParameter NullableInt(
            string name,
            int? value)
        {
            return new SqlParameter(name, SqlDbType.Int)
            {
                Value = value.HasValue
                    ? value.Value
                    : DBNull.Value
            };
        }

        private static SqlParameter NullableVarchar(
            string name,
            int size,
            string? value)
        {
            return new SqlParameter(
                name,
                SqlDbType.VarChar,
                size)
            {
                Value = string.IsNullOrWhiteSpace(value)
                    ? DBNull.Value
                    : value
            };
        }
    }
}
