using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveTypes.CreateLeaveType
{
    public record CreateLeaveTypeCommand(
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