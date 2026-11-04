using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Payroll.PayrollRuns.Commands.CalculatePayrollRun;
using HRMS.Domain.Entities.Payroll;
using HRMS.Domain.Entities.Payroll.Enums;
using HRMS.Infrastructure.Mappers.Payroll;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    public class PayrollRepository(ISqlExecutor sqlExecutor) : IPayrollRepository
    {
        public async Task<EmployeeCompensation?> GetActiveCompensationAsync(int employeeId, int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryMultipleAsync(
                "EmployeeCompensations_GetActive",
                EmployeeCompensationMapper.MapAsync,
                cancellationToken,
                Int("EmployeeId", employeeId),
                Int("OrganizationId", organizationId)
            );
        }

        public async Task<bool> SetCompensationAsync(EmployeeCompensation compensation, CancellationToken cancellationToken)
        {
            var allowancesParameter = CreateAllowanceDataTable(compensation.Allowances);
            var deductionsParameter = CreateDeductionsDataTable(compensation.Deductions);
            var parameters = new[]
            {
                Int("@OrganizationId", compensation.OrganizationId),
                Int("@EmployeeId", compensation.EmployeeId),
                new SqlParameter("@BasicSalary", compensation.BasicSalary),
                new SqlParameter("@Currency", compensation.Currency),
                new SqlParameter("@PayFrequency", (int)compensation.PayFrequency),
                DateTime2("@EffectiveFrom", compensation.EffectiveFrom),
                allowancesParameter,
                deductionsParameter
            };

            var id = await sqlExecutor.ExecuteWithScalarIntAsync(
                "EmployeeCompensations_SetCurrent",
                cancellationToken,
                parameters
                );

            return id > 0;
        }

        public async Task<bool> PayrollRunExistsAsync(
            int organizationId,
            int year,
            int? month,
            int? week,
            PayFrequency payFrequency,
            CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.PayrollRuns_Exists",
                cancellationToken,
                Int("@OrganizationId", organizationId),
                Int("@Year", year),
                NullableInt("@Month", month),
                NullableInt("@Week", week),
                Int("@PayFrequency", (int)payFrequency)
            );
        }

        public async Task<int> CreatePayrollRunAsync(PayrollRun run, CancellationToken cancellationToken)
        {
            var itemsParameter = CreatePayrollRunItemsDataTable(run.Items);
            var parameters = new[]
            {
                Int("@OrganizationId", run.OrganizationId),
                Int("@PayFrequency", (int)run.PayFrequency),
                Int("@Year", run.Year),
                NullableInt("@Month", run.Month),
                NullableInt("@Week", run.Week),
                Date("@PeriodStartDate", run.PeriodStartDate),
                Date("@PeriodEndDate", run.PeriodEndDate),
                Int("@Status", (int)run.Status),
                new SqlParameter("@TotalGrossSalary", run.TotalGrossSalary),
                new SqlParameter("@TotalAllowances", run.TotalAllowances),
                new SqlParameter("@TotalDeductions", run.TotalDeductions),
                new SqlParameter("@TotalNetSalary", run.TotalNetSalary),
                itemsParameter
            };
            return await sqlExecutor.ExecuteWithScalarIntAsync(
                "dbo.PayrollRuns_Create",
                cancellationToken,
                parameters
            );
        }

        public async Task<IReadOnlyList<EmployeePayrollCalculationDto>> GetEmployeesForCalculationAsync(
            int organizationId,
            PayFrequency payFrequency,
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "dbo.Payroll_GetEmployeesForCalculation",
                EmployeePayrollCalculationMapper.Map,
                cancellationToken,
                Int("@OrganizationId", organizationId),
                Int("@PayFrequency", (int)payFrequency),
                Date("@StartDate", startDate),
                Date("@EndDate", endDate)
            );
        }



        private static SqlParameter CreateAllowanceDataTable(IEnumerable<CompensationAllowance> allowances)
        {
            var table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            foreach (var allowance in allowances)
            {
                table.Rows.Add(allowance.Title, allowance.Amount);
            }
            return new SqlParameter("@Allowances", SqlDbType.Structured)
            {
                TypeName = "dbo.CompensationAllowanceInput",
                Value = table
            };
        }

        private static SqlParameter CreateDeductionsDataTable(IEnumerable<CompensationDeduction> deductions)
        {
            var table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            foreach (var deduction in deductions)
            {
                table.Rows.Add(deduction.Title, deduction.Amount);
            }
            return new SqlParameter("@Deductions", SqlDbType.Structured)
            {
                TypeName = "dbo.CompensationDeductionInput",
                Value = table
            };
        }

        private static SqlParameter CreatePayrollRunItemsDataTable(IEnumerable<PayrollRunItem> items)
        {
            var table = new DataTable();
            table.Columns.Add("EmployeeId", typeof(int));
            table.Columns.Add("BasicSalary", typeof(decimal));
            table.Columns.Add("TotalWorkingDays", typeof(int));
            table.Columns.Add("UnpaidDays", typeof(int));
            table.Columns.Add("DailyRate", typeof(decimal));
            table.Columns.Add("AttendanceDeductionAmount", typeof(decimal));
            table.Columns.Add("TotalAllowances", typeof(decimal));
            table.Columns.Add("TotalFixedDeductions", typeof(decimal));
            table.Columns.Add("GrossSalary", typeof(decimal));
            table.Columns.Add("NetSalary", typeof(decimal));
            foreach (var item in items)
            {
                table.Rows.Add(
                    item.EmployeeId,
                    item.BasicSalary,
                    item.TotalWorkingDays,
                    item.UnpaidDays,
                    item.DailyRate,
                    item.AttendanceDeductionAmount,
                    item.TotalAllowances,
                    item.TotalFixedDeductions,
                    item.GrossSalary,
                    item.NetSalary
                );
            }
            return new SqlParameter("@Items", SqlDbType.Structured)
            {
                TypeName = "dbo.PayrollRunItemInput",
                Value = table
            };
        }


    }
}
