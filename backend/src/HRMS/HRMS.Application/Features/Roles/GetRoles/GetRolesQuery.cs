using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.GetRoles
{
    public record GetRolesQuery() : IQuery<IReadOnlyList<GetRoleResponse>>;
}
