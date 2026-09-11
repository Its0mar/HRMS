using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Roles.GetRoles
{
    public class GetRolesHandler : IQueryHandler<GetRolesQuery, IReadOnlyList<GetRoleResponse>>
    {
        private readonly IRolesRepository _rolesRepository;

        public GetRolesHandler(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<ErrorOr<IReadOnlyList<GetRoleResponse>>> HandleAsync(GetRolesQuery query, CancellationToken cancellationToken)
        {
            var roles = await _rolesRepository.GetAllWithPermsAsync(query.OrganizationId, cancellationToken);

            var rolesResponse = roles.Select(role => new GetRoleResponse(
                role.Id!.Value,
                role.Name,
                role.Permissions
                .Select(permission => permission.Code).ToList())).ToList();

            return rolesResponse;
        }
    }
}
