using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence.Models;

namespace HRMS.Application.Features.Leaves.LeaveBalances.GetLeaveBalances
{
    public record GetLeaveBalancesQuery(
        int Year,
        int EmployeeId,
        int OrganizationiD) : IQuery<IReadOnlyList<MyLeaveBalancesResponse>>;
}
