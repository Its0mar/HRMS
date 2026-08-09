using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Permissions.GetPermissionOptions
{
    public sealed record GetPermissionOptionsQuery : IQuery<IReadOnlyList<PermissionOptionResponse>>;
}
