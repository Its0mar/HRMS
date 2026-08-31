using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveRequests.GetOrganizationLeaveRequests
{
    internal class GetOrganizationLeaveRequestsHandler(ILeaveRepository leaveRepository, ICurrentUser currentUser)
        : IQueryHandler<GetOrganizationLeaveRequestsQuery, IReadOnlyList<GetOrganizationLeaveRequestsResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<GetOrganizationLeaveRequestsResponse>>> HandleAsync(GetOrganizationLeaveRequestsQuery query, CancellationToken cancellationToken)
        {
            var list =  await leaveRepository.GetOrganizationLeaveRequestsAsync(currentUser.OrganizationId, query.Status, cancellationToken);

            return list.ToList();
        }
    }
}
