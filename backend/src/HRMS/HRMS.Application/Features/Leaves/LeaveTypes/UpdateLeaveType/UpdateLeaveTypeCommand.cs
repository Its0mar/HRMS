using HRMS.Application.Abstractions.Messaging;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Leaves.LeaveTypes.UpdateLeaveType
{
    public record UpdateLeaveTypeCommand(
        int Id,
        string Name,
        string Code,
        int DefaultDaysPerYear,
        bool IsPaid,
        bool RequiresApproval,
        int OrganizationId
        ) : ICommand<bool>, ICacheEvictingCommand
    {
        public string CacheKeyToEvict => $"leavetypes:org:{OrganizationId}";

    }
}
