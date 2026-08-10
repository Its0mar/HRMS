using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Roles.UpdateRole
{
    public sealed class UpdateRoleHandler : ICommandHandler<UpdateRoleCommand, bool>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateRoleHandler(IRolesRepository rolesRepository, ICurrentUser currentUser)
        {
            _rolesRepository = rolesRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<bool>> HandleAsync(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var role = await _rolesRepository.GetByIdAsync(
                command.Id,
                _currentUser.OrganizationId,
                cancellationToken);

            if (role is null)
            {
                return Error.NotFound(
                    "Role.NotFound",
                    "The role was not found.");
            }

            role.UpdateName(command.Name.Trim());

            var result =
                await _rolesRepository.UpdateWithPermissionsAsync(
                    role,
                    command.PermissionIds,
                    cancellationToken);

            if (result != 1)
            {
                return Error.Failure(
                    "Role.UpdateFailed",
                    "The role could not be updated.");
            }

            return true;
        }
    }
}
