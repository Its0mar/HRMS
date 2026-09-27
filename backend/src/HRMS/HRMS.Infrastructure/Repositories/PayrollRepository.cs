using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Payroll;
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
            var parameters = new[]
            {
                Int("@OrganizationId", compensation.OrganizationId),
                Int("@EmployeeId", compensation.EmployeeId),
                new SqlParameter("@BasicSalary", compensation.BasicSalary),
                new SqlParameter("@Currency", compensation.Currency),
                new SqlParameter("@PayFrequency", (int)compensation.PayFrequency),
                DateTime2("@EffectiveFrom", compensation.EffectiveFrom),
                allowancesParameter
            };

            var id = await sqlExecutor.ExecuteWithScalarIntAsync(
                "EmployeeCompensations_SetCurrent",
                cancellationToken,
                parameters
                );

            return id > 0;
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
    }
}
