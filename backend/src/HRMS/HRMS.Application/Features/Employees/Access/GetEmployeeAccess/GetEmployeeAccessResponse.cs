namespace HRMS.Application.Features.Employees.Access.GetEmployeeAccess
{
    public record GetEmployeeAccessResponse(
        int UserId,
        int EmployeeId,
        int RoleId,
        string UserName);
}
