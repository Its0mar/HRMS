using HRMS.Application.Abstractions.Persistence.Models;
using HRMS.Domain.Entities.Attendance;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IAttendanceCorrectionsRepository
    {
        public Task<IReadOnlyList<AttendanceCorrectionResponse>> GetOrganizationRecordsAsync(int organizationId, int status, CancellationToken cancellationToken);
        public Task<AttendanceCorrection?> GetByIdAsync(int Id, CancellationToken cancellationToken);
        public Task ApproveOrRejectCorrection(AttendanceCorrection correction, CancellationToken cancellationToken);

    }
}
