
namespace HRMS.Application.Features.Roles.GetRoleDetails
{
    public sealed record GetRoleDetailsResponse(
        int Id,
        string Name,
        IReadOnlyList<int> PermissionIds);
}
