using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Leaves;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using static HRMS.Infrastructure.Persistence.SqlParams;

namespace HRMS.Infrastructure.Repositories
{
    public sealed class LeaveRepository(ISqlExecutor sqlExecutor) : ILeaveRepository
    {
        public async Task<bool> CreateLeaveTypeAsync(LeaveType leaveType, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.LeaveTypes_Create",
                cancellationToken,
                Int("@OrganizationId", leaveType.OrganizationId),
                VarChar("@Name", 100, leaveType.Name),
                VarChar("@Code", 30, leaveType.Code),
                Int("@DefaultDaysPerYear", leaveType.DefaultDaysPerYear),
                Bit("@IsPaid", leaveType.IsPaid),
                Bit("@RequiresApproval", leaveType.RequiresApproval)
                );
        }

        public async Task<IReadOnlyList<LeaveType>> GetForOrganizationAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "dbo.LeaveTypes_GetForOrganization",
                Map,
                cancellationToken,
                Int("@OrganizationId", organizationId)
                );
        }

        public async Task<bool> NameOrCodeExistAsync(string name, string code, int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.LeaveTypes_NameOrCodeExist",
                cancellationToken,
                NullableVarChar("@Name", 100, name),
                NullableVarChar("@Code", 20, code),
                Int("@OrganizationId", organizationId)
                );
        }

        public async Task<LeaveType?> GetByIdAsync(int id, int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.LeaveTypes_GetById",
                Map,
                cancellationToken,
                Int("@Id", id),
                Int("@OrganizationId", organizationId));
        }

        public async Task<bool> UpdateLeaveTypeAsync(LeaveType leaveType, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.LeaveTypes_Update",
                cancellationToken,
                Int("@Id", leaveType.Id!.Value),
                Int("@OrganizationId", leaveType.OrganizationId),
                VarChar("@Name", 100, leaveType.Name),
                VarChar("@Code", 20, leaveType.Code),
                Int("@DefaultDaysPerYear", leaveType.DefaultDaysPerYear),
                Bit("@IsPaid", leaveType.IsPaid),
                Bit("@RequiresApproval", leaveType.RequiresApproval));
        }


        private LeaveType Map(SqlDataReader reader)
        {
            return LeaveType.Restore(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("OrganizationId")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Code")),
                reader.GetInt32(reader.GetOrdinal("DefaultDaysPerYear")),
                reader.GetBoolean(reader.GetOrdinal("IsPaid")),
                reader.GetBoolean(reader.GetOrdinal("RequiresApproval"))
                );
        }
    }
}