using HRMS.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Application.Features.Departments.CreateDepartment
{
    public sealed record CreateDepartmentCommand(
        string Name,
        string Code,
        string? Description,
        int? ManagerId)
    : ICommand<CreateDepartmentResponse>;
}
