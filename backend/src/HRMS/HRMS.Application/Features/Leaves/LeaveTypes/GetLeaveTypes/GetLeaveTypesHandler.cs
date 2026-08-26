using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Leaves.LeaveTypes.GetLeaveTypes
{
    public sealed class GetLeaveTypesHandler(ILeaveRepository leaveRepository) : IQueryHandler<GetLeaveTypesQuery, IReadOnlyList<LeaveTypeResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<LeaveTypeResponse>>> HandleAsync(GetLeaveTypesQuery query, CancellationToken cancellationToken)
        {
            var list = await leaveRepository.GetForOrganizationAsync(query.OrganizationId, cancellationToken);

            return list.Select(l => new LeaveTypeResponse(l.Id ?? -1, l.Name, l.Code, l.DefaultDaysPerYear, l.IsPaid, l.RequiresApproval)).ToList();
        }
    }
}
