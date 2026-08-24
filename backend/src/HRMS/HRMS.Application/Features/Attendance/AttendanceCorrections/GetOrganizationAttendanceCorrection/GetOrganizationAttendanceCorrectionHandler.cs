using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Application.Abstractions.Persistence.Models;

namespace HRMS.Application.Features.Attendance.AttendanceCorrections.GetOrganizationAttendanceCorrection
{
    public sealed class GetOrganizationAttendanceCorrectionHandler(
        IAttendanceCorrectionsRepository attendanceCorrectionsRepository,
        ICurrentUser currentUser) 
        : IQueryHandler<GetOrganizationAttendanceCorrectionQuery, IReadOnlyList<AttendanceCorrectionResoonse>>
    {
        public async Task<ErrorOr<IReadOnlyList<AttendanceCorrectionResoonse>>> HandleAsync(GetOrganizationAttendanceCorrectionQuery query, CancellationToken cancellationToken)
        {
            var records = await attendanceCorrectionsRepository.GetOrganizationRecordsAsync(currentUser.OrganizationId, (int)query.Status, cancellationToken);
            return records.ToList();
        }
    }
}
