using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.WorkSchedules.UpdateWorkSchedule
{
    public record UpdateWorkScheduleCommand(
            int Id,
            string Name,
            int GracePeriodMinutes,
            bool IsDefault
        ) : ICommand<bool>;
}
