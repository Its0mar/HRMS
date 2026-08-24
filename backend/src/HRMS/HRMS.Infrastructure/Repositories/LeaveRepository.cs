using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Leaves;
using HRMS.Infrastructure.Persistence;
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
    }
}