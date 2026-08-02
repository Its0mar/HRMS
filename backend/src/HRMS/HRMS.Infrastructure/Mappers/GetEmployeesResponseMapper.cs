using HRMS.Application.Features.Employees.GetEmployees;
using HRMS.Domain.Entities.Employees;
using HRMS.Domain.Entities.Employees.Enums;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers
{
    public static class GetEmployeesResponseMapper
    {
        public static GetEmployeesResponse Map(SqlDataReader reader)
        {
            return new GetEmployeesResponse(
                Id: reader.GetInt32(
                    reader.GetOrdinal("Id")),

                EmployeeNumber: reader.GetString(
                    reader.GetOrdinal("EmployeeNumber")),

                FullName: reader.GetString(
                    reader.GetOrdinal("FullName")),

                WorkEmail: reader.GetString(
                    reader.GetOrdinal("WorkEmail")),

                DepartmentName: reader.GetString(
                    reader.GetOrdinal("DepartmentName")),

                PositionName: reader.GetString(
                    reader.GetOrdinal("PositionName")),

                EmploymentType: reader.GetString(
                    reader.GetOrdinal("EmploymentType")),

                EmploymentStatus: reader.GetString(
                    reader.GetOrdinal("EmploymentStatus"))
            );
        }
    }
}
