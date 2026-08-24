using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Attendance.GetOrganizationAttendance
{
    public sealed record GetOrganizationAttendanceQuery(
        DateOnly? Date = null,
        string? SearchTerm = null) : IQuery<IReadOnlyList<GetOrganizationAttendanceResponse>>;
}
