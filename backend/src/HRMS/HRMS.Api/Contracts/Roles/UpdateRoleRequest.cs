namespace HRMS.Api.Contracts.Roles
{
    public sealed record UpdateRoleRequest(
        string Name,
        List<int> PermissionIds);
}
