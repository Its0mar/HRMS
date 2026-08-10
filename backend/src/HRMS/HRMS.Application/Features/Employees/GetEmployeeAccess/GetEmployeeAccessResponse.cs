namespace HRMS.Application.Features.Employees.GetEmployeeAccess
{
    public record GetEmployeeAccessResponse(
        int UserId,
        int EmployeeId,
        int RoleId,
        string UserName);
}
