using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Roles.GetRolesOptions
{
    public sealed class GetRolesOptionsHandler : IQueryHandler<GetRolesOptionsQuery, IReadOnlyList<GetRolesOptionsResponse>>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly ICurrentUser _currentUser;

        public GetRolesOptionsHandler(IRolesRepository rolesRepository, ICurrentUser currentUser)
        {
            _rolesRepository = rolesRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<IReadOnlyList<GetRolesOptionsResponse>>> HandleAsync(GetRolesOptionsQuery query, CancellationToken cancellationToken)
        {
            var roles = await _rolesRepository.GetAllWithPermsAsync(_currentUser.OrganizationId, cancellationToken);

            return roles.Select(x => new GetRolesOptionsResponse(x.Id ?? 0, x.Name)).ToList();
        }
    }
}
