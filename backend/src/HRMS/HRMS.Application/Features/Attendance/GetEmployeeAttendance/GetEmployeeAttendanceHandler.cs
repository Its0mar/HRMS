using ErrorOr;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Attendance;

namespace HRMS.Application.Features.Attendance.GetEmployeeAttendance
{
    public class GetEmployeeAttendanceHandler(IAttendanceRepository attendanceRepository)
        : IQueryHandler<GetEmployeeAttendanceQuery, IReadOnlyList<GetEmployeeAttendanceResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<GetEmployeeAttendanceResponse>>> HandleAsync(GetEmployeeAttendanceQuery command, CancellationToken cancellationToken)
        {
            var records = await attendanceRepository.GetMyRecordsAsync(command.EmployeeId, cancellationToken);

            return records.Select(Map).ToList();
        }

        private GetEmployeeAttendanceResponse Map(AttendanceLog r)
        {
            var clockInIso = DateTime.SpecifyKind(r.ClockIn, DateTimeKind.Utc).ToString("o");

            string? clockOutIso = r.ClockOut.HasValue
                ? DateTime.SpecifyKind(r.ClockOut.Value, DateTimeKind.Utc).ToString("o")
                : null;

            return new GetEmployeeAttendanceResponse(
                r.Id!.Value,
                r.Date,
                clockInIso,
                clockOutIso,
                r.Status.ToString(),
                r.TotalMinutes,
                r.LateMinutes,
                r.OvertimeMinutes);
        }
    }

}