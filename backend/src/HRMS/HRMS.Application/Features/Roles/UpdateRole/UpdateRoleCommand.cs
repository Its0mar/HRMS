using HRMS.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Roles.UpdateRole
{
    public sealed record UpdateRoleCommand(
        int Id,
        string Name,
        List<int> PermissionIds)
        : ICommand<bool>;
}
