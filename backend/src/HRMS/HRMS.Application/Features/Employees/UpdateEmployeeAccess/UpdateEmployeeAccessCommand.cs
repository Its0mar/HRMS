using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Employees.UpdateEmployeeAccess
{
    public record UpdateEmployeeAccessCommand(
        int EmployeeId,
        string Username,
        int RoleId) : ICommand<bool>;
}
