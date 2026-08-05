using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.WorkSchedules.Common;

namespace HRMS.Application.Features.WorkSchedules.CreateWorkSchedules
{
    public record CreateWorkScheduleCommand(
        string Name,
        int GracePeriodMinutes,
        bool IsDefault,
        List<WorkScheduleDayDto> WorkScheduleDay
    ) : ICommand<int>;
}