using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Application.Features.Leaves.LeaveRequests;
using HRMS.Domain.Entities.Leaves;
using HRMS.Infrastructure.Mappers.Leaves;
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
                LeaveTypeMap,
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

        public async Task<LeaveType?> GetTypeByIdAsync(int id, int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.LeaveTypes_GetById",
                LeaveTypeMap,
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

        public async Task<List<MyLeaveBalancesResponse>> GetMyBalancesAsync(int year, int employeeId, int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "dbo.Leaves_GetMyBalances",
                MyLeaveBalancesResponseMapper.Map,
                cancellationToken,
                new SqlParameter("@Year", year),
                new SqlParameter("@EmployeeId", employeeId),
                new SqlParameter("@OrganizationId", organizationId));
        }
        public async Task<EmployeeLeaveBalance?> GeyMyBalanceAsync(int leaveTypeId, int year, int employeeId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryFirstOrDefaultAsync(
                "dbo.Leaves_GetMyBalance",
                EmployeeLeaveBalancesMap,
                cancellationToken,
                new SqlParameter("@LeaveTypeId", leaveTypeId),
                new SqlParameter("@Year", year),
                new SqlParameter("@EmployeeId", employeeId));
        }

        public async Task<bool> CreateEmployeeBalance(EmployeeLeaveBalance balance, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.EmployeeLeaveBalances_Create",
                cancellationToken,
                Int("@EmployeeId", balance.EmployeeId),
                Int("@LeaveTypeId", balance.LeaveTypeId),
                Int("@Year", balance.Year),
                new SqlParameter("@TotalEntitledDays", System.Data.SqlDbType.Decimal)
                {
                    Value = balance.TotalEntitledDays
                },
                new SqlParameter("@PendingDays", System.Data.SqlDbType.Decimal)
                {
                    Value = balance.PendingDays
                }

                );
        }

        public async Task<bool> UpdatePendingDaysAsync(EmployeeLeaveBalance balance, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.EmployeeLeaveBalances_UpdatePendingDays",
                cancellationToken,
                Int("@BalanceId", balance.Id ?? -1),
                Int("@EmployeeId", balance.EmployeeId),
                new SqlParameter("@PendingDays", System.Data.SqlDbType.Decimal)
                {
                    Value = balance.PendingDays
                });
        }

        public async Task<bool> CreateLeaveRequestAsync(LeaveRequest request, CancellationToken cancellationToken)
        {
            return await sqlExecutor.ExecuteScalarBoolAsync(
                "dbo.LeaveRequests_Create",
                cancellationToken,
                Int("@OrganizationId", request.OrganizationId),
                Int("@EmployeeId", request.EmployeeId),
                Int("@LeaveTypeId", request.LeaveTypeId),
                DateTime2("@StartDate", request.StartDate),
                DateTime2("@EndDate", request.EndDate),
                new SqlParameter("@TotalDays", System.Data.SqlDbType.Decimal)
                {
                    Value = request.TotalDays
                },
                VarChar("@Reason", 300, request.Reason));

                
        }

        public async Task<List<LeaveRequestResponse>> GetEmployeeLeaveRequestsAsync(int employeeId, int organizationId, CancellationToken cancellationToken)
        {
            return await sqlExecutor.QueryAsync(
                "LeaveRequests_GetMyRequests",
                LeaveRequestResponseMapper.Map,
                cancellationToken,
                Int("@EmployeeId", employeeId),
                Int("@OrganizationId", organizationId)
                );
        }


        private LeaveType LeaveTypeMap(SqlDataReader reader)
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

        private EmployeeLeaveBalance EmployeeLeaveBalancesMap(SqlDataReader reader)
        {
            return EmployeeLeaveBalance.Restore(
                 reader.GetInt32(reader.GetOrdinal("Id")),
                  reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                  reader.GetInt32(reader.GetOrdinal("LeaveTypeId")),
                  reader.GetInt32(reader.GetOrdinal("Year")),
                  reader.GetInt32(reader.GetOrdinal("TotalEntitledDays")),
                  reader.GetDecimal(reader.GetOrdinal("UsedDays")),
                  reader.GetDecimal(reader.GetOrdinal("PendingDays"))
                );
        }
    
    }
}