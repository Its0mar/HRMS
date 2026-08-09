
namespace HRMS.Application.Features.Permissions.GetPermissionOptions
{
    public sealed record PermissionOptionResponse(
        int Id,
        string Code,
        string Description);
}
