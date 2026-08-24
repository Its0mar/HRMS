using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Domain.Entities.Attendance;

namespace HRMS.Application.Features.Attendance.AttendanceCorrections.GetOrganizationAttendanceCorrection
{
    public sealed record GetOrganizationAttendanceCorrectionQuery(
            AttendanceCorrectionsStatus Status
        ) : IQuery<IReadOnlyList<AttendanceCorrectionResoonse>>;
}
