using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.WorkSchedules.Enums;

namespace HRMS.Application.Features.Attendance.ClockOut
{
    public sealed class ClockOutHandler(
        IAttendanceRepository attendanceRepository,
        IWorkScheduleRepository workScheduleRepository,
        ICurrentUser currentUser) : ICommandHandler<ClockOutCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(ClockOutCommand command, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var today = DateOnly.FromDateTime(now);

            var existingLog = await attendanceRepository.GetTodayLogForEmployeeAsync(command.employeeId, today, cancellationToken);
            if (existingLog is null)
            {
                return Error.Conflict("Attendance.NotClockedIn", "You have not clocked in for today.");
            }

            var workSchedule = await workScheduleRepository.GetEmployeeWorkScheduleByEmployeeId(command.employeeId, currentUser.OrganizationId, cancellationToken);
            var workDay = convertFromDayOfWeekToWorkDay(now.DayOfWeek);
            var todayScheduleDay = workSchedule?.Days.FirstOrDefault(d => d.WorkDay == workDay);
            var minimumMinutes = todayScheduleDay?.MinimumMinutesPerDay ?? 480; // Default to 480 mins (8 hrs) if null

            // Encapsulated Rich Domain Entity Method Execution
            var clockOutResult = existingLog.PerformClockOut(now, minimumMinutes);
            if (clockOutResult.IsError)
            {
                return clockOutResult.Errors;
            }

            return await attendanceRepository.ClockOutAsync(
                existingLog.Id!.Value,
                existingLog.ClockOut!.Value,
                existingLog.TotalMinutes!.Value,
                existingLog.OvertimeMinutes,
                cancellationToken);
        }

        private WorkDay convertFromDayOfWeekToWorkDay(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday) { return WorkDay.Sunday; }
            if (dayOfWeek == DayOfWeek.Monday) { return WorkDay.Monday; }
            if (dayOfWeek == DayOfWeek.Tuesday) { return WorkDay.Tuesday; }
            if (dayOfWeek == DayOfWeek.Wednesday) { return WorkDay.Wednesday; }
            if (dayOfWeek == DayOfWeek.Thursday) { return WorkDay.Thursday; }
            if (dayOfWeek == DayOfWeek.Friday) { return WorkDay.Friday; }
            return WorkDay.Saturday;
        }
    }
}
