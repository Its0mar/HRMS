using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Departments.UpdateDepartment
{
    public sealed record UpdateDepartmentCommand(
        int Id,
        string? Name,
        string? Description,
        int? ManagerEmployeeId
        ) : ICommand<bool>;
    
}
