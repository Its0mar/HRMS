using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Persistence
{
    public interface IPositionsRepository
    {
        Task<int> CreateAsync(Position position, CancellationToken cancellationToken);
        Task<bool> TitleExistsAsync(int organizationId, string title, CancellationToken cancellationToken);
        public Task<List<Position>> GetPositionsAsync(int organizationId, CancellationToken cancellationToken);

    }
}
