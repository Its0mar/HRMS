using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities.Roles;

namespace HRMS.Application.Features.Roles.CreateRole
{
    public class CreateRoleHandler : ICommandHandler<CreateRoleCommand, bool>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly ICurrentUser _currentUser;

        public CreateRoleHandler(IRolesRepository rolesRepository, ICurrentUser currentUser)
        {
            _rolesRepository = rolesRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<bool>> HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var role = new Role(command.Name, _currentUser.OrganizationId);

            var result = await _rolesRepository.CreateWithPermissionsAsync(role, command.PermissionIds, cancellationToken);

            if (result < 0)
            {
                return Error.Failure("failed to create a role");
            }

            return true;
        }
    }
}
