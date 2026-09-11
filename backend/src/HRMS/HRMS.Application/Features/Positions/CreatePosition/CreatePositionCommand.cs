using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Positions.CreatePosition
{
    public sealed record CreatePositionCommand(
        string Title,
        string? Description,
        int OrganizationId
        ) : ICommand<int>, ICacheEvictingCommand
    {
        public string CacheKeyToEvict => $"positions:org:{OrganizationId}";
    }
}