using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.WorkSchedules.Common;

namespace HRMS.Application.Features.WorkSchedules.UpdateWorkSchedule
{
    public record UpdateWorkScheduleCommand(
            int Id,
            string Name,
            int GracePeriodMinutes,
            bool IsDefault,
            List<WorkScheduleDayDto> WorkScheduleDays
        ) : ICommand<bool>;
}
