using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Persistence.Models;

namespace HRMS.Application.Features.Leaves.LeaveBalances.GetLeaveBalances
{
    public sealed class GetLeaveBalancesHandler(ILeaveRepository leaveRepository)
        : IQueryHandler<GetLeaveBalancesQuery, IReadOnlyList<MyLeaveBalancesResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<MyLeaveBalancesResponse>>> HandleAsync(GetLeaveBalancesQuery query, CancellationToken cancellationToken)
        {
            var result = await leaveRepository.GetMyBalancesAsync(query.Year, query.EmployeeId, query.OrganizationiD, cancellationToken);

            return result ?? [];
        }
    }
}
