using HRMS.Application.Abstractions.Persistence.Models;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IAttendanceCorrectionsRepository
    {
        public Task<IReadOnlyList<OrganizationAttendanceCorrection>> GetOrganizationRecordsAsync(int organizationId, int status, CancellationToken cancellationToken);
    }
}
