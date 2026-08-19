using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.GetEmployeeAttendance
{
    public record GetEmployeeAttendanceQuery(int EmployeeId) : IQuery<IReadOnlyList<GetEmployeeAttendanceResponse>>;
}
