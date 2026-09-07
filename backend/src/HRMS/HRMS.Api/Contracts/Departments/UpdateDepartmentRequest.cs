namespace HRMS.Api.Contracts.Departments;

public record UpdateDepartmentRequest(
    int Id,
    string? Name,
    string? Description,
    int? ManagerEmployeeId
);
