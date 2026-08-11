using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.Permissions.GetPermissionOptions
{
    public sealed record GetPermissionOptionsQuery : IQuery<IReadOnlyList<PermissionOptionResponse>>;
}
