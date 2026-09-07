namespace HRMS.Api.Contracts.Roles;

public record CreateRoleRequest(
    string Name,
    List<int> PermissionIds
);
