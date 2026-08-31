
using HRMS.Domain.Entities.Leaves;

namespace HRMS.Application.Features.Dashboard.Dtos
{
    public record RecentLeaveRequestDto
    (
        int Id,
        string TypeName,
        DateOnly StartDate,
        DateOnly EndDate,
        LeaveRequestStatus Status
    );
}
