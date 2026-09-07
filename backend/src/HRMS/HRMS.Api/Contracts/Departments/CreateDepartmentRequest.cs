namespace HRMS.Api.Contracts.Departments;

public record CreateDepartmentRequest(
    string Name,
    string Code,
    string? Description,
    int? ManagerId
);
