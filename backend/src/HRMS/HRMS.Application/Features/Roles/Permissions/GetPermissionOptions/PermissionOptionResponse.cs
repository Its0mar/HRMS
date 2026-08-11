namespace HRMS.Application.Features.Roles.Permissions.GetPermissionOptions
{
    public sealed record PermissionOptionResponse(
        int Id,
        string Code,
        string Description);
}
