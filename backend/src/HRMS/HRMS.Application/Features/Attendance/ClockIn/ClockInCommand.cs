using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.ClockIn
{
    public record ClockInCommand(
        int employeeId) : ICommand<bool>;
}
