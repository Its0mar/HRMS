using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using System;

namespace HRMS.Application.Features.Attendance.GetOrganizationAttendance
{
    public sealed class GetOrganizationAttendanceHandler(
         IAttendanceRepository attendanceRepository,
         ICurrentUser currentUser) : IQueryHandler<GetOrganizationAttendanceQuery, IReadOnlyList<GetOrganizationAttendanceResponse>>
    {
        public async Task<ErrorOr<IReadOnlyList<GetOrganizationAttendanceResponse>>> HandleAsync(
            GetOrganizationAttendanceQuery query, CancellationToken cancellationToken)
        {
            var list = await attendanceRepository.GetOrganizationRecordsAsync(
                currentUser.OrganizationId,
                query.Date,
                query.SearchTerm,
                cancellationToken);

            return list.ToList();
        }
    }
}
