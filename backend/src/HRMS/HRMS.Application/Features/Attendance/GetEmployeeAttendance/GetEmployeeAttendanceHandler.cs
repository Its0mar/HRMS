using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Features.Attendance.GetUserAttendance;

namespace HRMS.Application.Features.Attendance.GetEmployeeAttendance
{
    public class GetEmployeeAttendanceHandler(IAttendanceRepository attendanceRepository)
        : IQueryHandler<GetEmployeeAttendanceQuery, IReadOnlyList<GetEmployeeAttendanceResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<GetEmployeeAttendanceResponse>>> HandleAsync(GetEmployeeAttendanceQuery command, CancellationToken cancellationToken)
        {
            var records = await attendanceRepository.GetMyRecordsAsync(command.EmployeeId, cancellationToken);

            return records.Select(r => new GetEmployeeAttendanceResponse(r.Date, r.ClockIn, r.ClockOut, r.Status.ToString(), r.TotalMinutes, r.LateMinutes, r.OvertimeMinutes)).ToList();
        }
    }
}