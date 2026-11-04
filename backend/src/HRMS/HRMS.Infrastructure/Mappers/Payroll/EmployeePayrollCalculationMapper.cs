using HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Payroll
{
    public static class EmployeePayrollCalculationMapper
    {
        public static EmployeePayrollCalculationDto Map(SqlDataReader reader)
        {
            return new EmployeePayrollCalculationDto
            {
                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                BasicSalary = reader.GetDecimal(reader.GetOrdinal("BasicSalary")),
                TotalAllowances = reader.GetDecimal(reader.GetOrdinal("TotalAllowances")),
                TotalFixedDeductions = reader.GetDecimal(reader.GetOrdinal("TotalFixedDeductions")),
                UnpaidDays = reader.GetInt32(reader.GetOrdinal("UnpaidDays")),
                TotalWorkingDays = reader.GetInt32(reader.GetOrdinal("TotalWorkingDays"))
            };
        }
    }
}