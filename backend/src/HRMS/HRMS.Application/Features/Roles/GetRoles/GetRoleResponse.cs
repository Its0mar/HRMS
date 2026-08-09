namespace HRMS.Application.Features.Roles.GetRoles
{
    public record GetRoleResponse(
        int Id,
        string Name,
        IReadOnlyList<string> Permissions);
}
