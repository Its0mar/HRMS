using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Positions.GetPositions
{
    public sealed record GetPositionsQuery() : IQuery<List<GetPositionResponse>>;
    public class GetPositionsQueryHandler
        : IQueryHandler<GetPositionsQuery, List<GetPositionResponse>>
    {
        private readonly IPositionsRepository _positionsRepository;
        private readonly ICurrentUser _currentUser;

        public GetPositionsQueryHandler(IPositionsRepository positionsRepository, ICurrentUser currentUser)
        {
            _positionsRepository = positionsRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<List<GetPositionResponse>>> HandleAsync(GetPositionsQuery query, CancellationToken cancellationToken)
        {
            var positions = await _positionsRepository.GetPositionsAsync(_currentUser.OrganizationId, cancellationToken);

            return positions.Select(p => new GetPositionResponse(p.Id ?? -1, p.Title)).ToList();
        }
    }
}
