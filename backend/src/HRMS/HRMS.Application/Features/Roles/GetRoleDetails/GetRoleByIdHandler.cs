using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;


namespace HRMS.Application.Features.Roles.GetRoleDetails
{
    public sealed class GetRoleByIdHandler
    : IQueryHandler<GetRoleByIdQuery, GetRoleDetailsResponse>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly ICurrentUser _currentUser;

        public GetRoleByIdHandler(
            IRolesRepository rolesRepository,
            ICurrentUser currentUser)
        {
            _rolesRepository = rolesRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<GetRoleDetailsResponse>> HandleAsync(
            GetRoleByIdQuery query,
            CancellationToken cancellationToken)
        {
            var role = await _rolesRepository.GetByIdAsync(
                query.Id,
                _currentUser.OrganizationId,
                cancellationToken);

            if (role is null)
            {
                return Error.NotFound(
                    "Role.NotFound",
                    "The role was not found.");
            }

            return new GetRoleDetailsResponse(
                role.Id!.Value,
                role.Name,
                role.Permissions
                    .Select(permission => permission.Id)
                    .ToList());
        }
    }
}
