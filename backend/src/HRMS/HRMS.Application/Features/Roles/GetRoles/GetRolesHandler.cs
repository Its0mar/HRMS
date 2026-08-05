
using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Roles.GetRoles
{
    public class GetRolesHandler : IQueryHandler<GetRolesQuery, IReadOnlyList<GetRoleResponse>>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly ICurrentUser _currentUser;

        public GetRolesHandler(IRolesRepository rolesRepository, ICurrentUser currentUser)
        {
            _rolesRepository = rolesRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<IReadOnlyList<GetRoleResponse>>> HandleAsync(GetRolesQuery query, CancellationToken cancellationToken)
        {
            var roles = await _rolesRepository.GetAllAsync(_currentUser.OrganizationId, cancellationToken);
            var rolesResponse = roles.Select(role => new GetRoleResponse(role.Id ?? 0, role.Name)).ToList();

            return rolesResponse;
        }
    }
}
