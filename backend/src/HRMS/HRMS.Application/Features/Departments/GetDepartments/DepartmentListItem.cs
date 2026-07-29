namespace HRMS.Application.Features.Departments.GetDepartments
{
    public sealed record class DepartmentListItem(
        int Id,
        string Name,
        string Code,
        string Description= "",
        string ManagerName = "",
        int ManagerEmployeeId = 0

        );
}
