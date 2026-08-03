using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.WorkSchedules.CreateWorkSchedules
{
    public record class CreateWorkScheduleCommand(
        string Name,
        int GracePeriodMinutes,
        bool IsDefault,
        List<WorkScheduleDayDto> WorkScheduleDay
    ) : ICommand<int>;
}