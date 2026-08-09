
using HRMS.Application.Abstractions.Messaging;
using HRMS.Domain.Entities.Roles;

namespace HRMS.Application.Features.Roles.CreateRole
{
    public record CreateRoleCommand(
        string Name,
        List<int> PermissionIds) : ICommand<bool>;
}
