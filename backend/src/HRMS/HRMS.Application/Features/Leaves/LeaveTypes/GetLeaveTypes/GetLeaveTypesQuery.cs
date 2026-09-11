using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Leaves.LeaveTypes.GetLeaveTypes
{
    public sealed record GetLeaveTypesQuery(int OrganizationId)
        : IQuery<IReadOnlyList<LeaveTypeResponse>>, ICachedQuery
    {
        public string CacheKey => $"leavetypes:org:{OrganizationId}";
        public TimeSpan? Expiration => TimeSpan.FromMinutes(30);
    }
}

