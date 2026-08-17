using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Attendance.GetEmployeeAttendance;

namespace HRMS.Application.Features.Attendance.GetUserAttendance
{
    public record GetEmployeeAttendanceQuery(int EmployeeId) : IQuery<IReadOnlyList<GetEmployeeAttendanceResponse>>;
}
