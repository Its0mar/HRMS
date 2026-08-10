using HRMS.Application.Abstractions.Messaging;
using HRMS.Domain.Entities.Roles;

namespace HRMS.Application.Features.Roles.GetRolesOptions
{
    public record GetRolesOptionsQuery(int organisationId) : IQuery<IReadOnlyList<GetRolesOptionsResponse>>;
}
