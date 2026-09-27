
using HRMS.Domain.Entities.Payroll;
using HRMS.Domain.Entities.Payroll.Enums;
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Mappers.Payroll
{
    public static class EmployeeCompensationMapper
    {
        public static async Task<EmployeeCompensation?> MapAsync(SqlDataReader reader, CancellationToken ct = default)
        {
            if (!await reader.ReadAsync(ct))
                return null;

            var compensation = EmployeeCompensation.Restore(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                organizationId: reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                employeeId: reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                basicSalary: reader.GetDecimal(reader.GetOrdinal("Salary")),
                currency: reader.GetString(reader.GetOrdinal("Currency")),
                payFrequency: (PayFrequency)reader.GetInt32(reader.GetOrdinal("PayFrequency")),
                effectiveFrom: reader.GetDateTime(reader.GetOrdinal("EffectiveDate")),
                effectiveTo: reader.IsDBNull(reader.GetOrdinal("EffectiveTo")) ? null : reader.GetDateTime(reader.GetOrdinal("EffectiveTo")),
                isCurrent: reader.GetBoolean(reader.GetOrdinal("IsCurrent")),
                createdAt: reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
            );

            if (await reader.NextResultAsync(ct))
            {
                var allowances = new List<CompensationAllowance>();
                while (await reader.ReadAsync(ct))
                {
                    var allowance = CompensationAllowance.Restore(
                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        compensationId: reader.GetInt32(reader.GetOrdinal("CompensationId")),
                        title: reader.GetString(reader.GetOrdinal("Title")),
                        amount: reader.GetDecimal(reader.GetOrdinal("Amount"))
                    );
                    allowances.Add(allowance);
                }
                compensation.AddAllowances(allowances);
            }

            return compensation;
        }
    }
}
