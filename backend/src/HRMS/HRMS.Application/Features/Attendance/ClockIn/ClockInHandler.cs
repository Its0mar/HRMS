using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Attendance;
using HRMS.Domain.Entities.WorkSchedules.Enums;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Attendance.ClockIn
{
    public sealed class ClockInHandler(
        IAttendanceRepository attendanceRepository,
        IWorkScheduleRepository workScheduleRepository,
        ICurrentUser currentUser,
        ILogger<ClockInHandler> logger) : ICommandHandler<ClockInCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(ClockInCommand command, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var today = DateOnly.FromDateTime(now);

            // Check if employee clocked in today
            var existingLog = await attendanceRepository.GetTodayLogForEmployeeAsync(command.employeeId, today, cancellationToken);
            if (existingLog is not null)
            {
                logger.LogWarning("Employee {EmployeeId} attempted duplicate clock-in for today ({Date})", command.employeeId, today);
                return Error.Conflict("Attendance.AlreadyClockedIn", "You have already clocked in for today.");
            }

            // Get current workschedule for employee
            var workSchedule = await workScheduleRepository.GetEmployeeWorkScheduleByEmployeeId(command.employeeId, currentUser.OrganizationId, cancellationToken);

            var workDay = ConvertToWorkDay(now.DayOfWeek);
            var todayScheduleDay = workSchedule?.Days.FirstOrDefault(d => d.WorkDay == workDay);
            if (workSchedule is null || workSchedule.Id is null || todayScheduleDay is null || !todayScheduleDay.IsWorkingDay)
            {
                logger.LogWarning("Employee {EmployeeId} attempted clock-in on non-working day ({Date})", command.employeeId, today);
                return Error.Validation("Attendance.OffDay", "No work shift scheduled for today.");
            }

            // Calculate Status & LateMinutes
            var shiftStartTime = today.ToDateTime(todayScheduleDay.StartTime!.Value);
            var lateThreshold = shiftStartTime.AddMinutes(workSchedule.GracePeriodMinutes);
            var status = AttendanceStatus.Present;
            var lateMinutes = 0;
            if (now > lateThreshold)
            {
                status = AttendanceStatus.Late;
                lateMinutes = (int)(now - shiftStartTime).TotalMinutes;
            }

            var attendanceLog = new AttendanceLog(command.employeeId, workSchedule.Id.Value, currentUser.OrganizationId, status, lateMinutes);
            
            var success = await attendanceRepository.ClockInAsync(attendanceLog, cancellationToken) > 0;
            if (success)
            {
                logger.LogInformation("Employee {EmployeeId} clocked in successfully at {ClockInTime} (Status: {Status}, LateMinutes: {LateMinutes})", command.employeeId, now, status, lateMinutes);
            }

            return success;
        }

        private static WorkDay ConvertToWorkDay(DayOfWeek dayOfWeek) => dayOfWeek switch
        {
            DayOfWeek.Sunday => WorkDay.Sunday,
            DayOfWeek.Monday => WorkDay.Monday,
            DayOfWeek.Tuesday => WorkDay.Tuesday,
            DayOfWeek.Wednesday => WorkDay.Wednesday,
            DayOfWeek.Thursday => WorkDay.Thursday,
            DayOfWeek.Friday => WorkDay.Friday,
            DayOfWeek.Saturday => WorkDay.Saturday,
            _ => WorkDay.Sunday
        };
    }
}
