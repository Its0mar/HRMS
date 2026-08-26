using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetMyLeaveRequests
{
    public sealed class GetMyLeaveRequestsHandler(ILeaveRepository leaveRepository)
        : IQueryHandler<GetMyLeaveRequestsQuery, IReadOnlyList<LeaveRequestResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<LeaveRequestResponse>>> HandleAsync(GetMyLeaveRequestsQuery query, CancellationToken cancellationToken)
        {
            var list = await leaveRepository.GetEmployeeLeaveRequestsAsync(query.EmployeeId, query.OrganizationId, cancellationToken);

            return list;
        }
    }
}
