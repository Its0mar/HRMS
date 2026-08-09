using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Infrastructure.Mappers.Roles
{
    public sealed record RolePermissionRow(
        int RoleId,
        string RoleName,
        int? PermissionId,
        string? PermissionCode);
}
