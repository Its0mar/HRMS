
using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.ClockOut
{
    public sealed record class ClockOutCommand(
        int employeeId) : ICommand<bool>;
}
