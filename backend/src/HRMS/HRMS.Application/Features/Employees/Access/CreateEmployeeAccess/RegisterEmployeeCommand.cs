using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Employees.Access.CreateEmployeeAccess
{
    public sealed record RegisterEmployeeCommand(int EmployeeId, string UserName, int RoleId, string Password, string ConfirmPassword) : ICommand<int>;
}
