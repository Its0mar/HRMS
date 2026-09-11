using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Positions.GetPositions
{
    public sealed record GetPositionsQuery(int OrganizationId) : IQuery<List<GetPositionResponse>>, ICachedQuery
    {
        public string CacheKey => $"positions:org:{OrganizationId}";
        public TimeSpan? Expiration => TimeSpan.FromHours(1);
    }

    public class GetPositionsQueryHandler
        : IQueryHandler<GetPositionsQuery, List<GetPositionResponse>>
    {
        private readonly IPositionsRepository _positionsRepository;

        public GetPositionsQueryHandler(IPositionsRepository positionsRepository)
        {
            _positionsRepository = positionsRepository;
        }

        public async Task<ErrorOr<List<GetPositionResponse>>> HandleAsync(GetPositionsQuery query, CancellationToken cancellationToken)
        {
            var positions = await _positionsRepository.GetPositionsAsync(query.OrganizationId, cancellationToken);
            return positions.Select(p => new GetPositionResponse(p.Id ?? -1, p.Title)).ToList();
        }
    }
}
