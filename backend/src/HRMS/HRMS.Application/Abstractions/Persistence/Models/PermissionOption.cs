using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Abstractions.Persistence.Models
{
    public sealed record PermissionOption(
        int Id,
        string Code,
        string Description);
}
