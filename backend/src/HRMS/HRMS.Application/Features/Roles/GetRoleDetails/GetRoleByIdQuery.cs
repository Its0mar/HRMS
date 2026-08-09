
using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Roles.GetRoleDetails
{
    public sealed record GetRoleByIdQuery(int Id) : IQuery<GetRoleDetailsResponse>;
}
