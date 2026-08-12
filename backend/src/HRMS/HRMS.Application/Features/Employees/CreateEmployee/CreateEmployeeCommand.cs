using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Models;
using HRMS.Domain.Entities.Employees.Enums;

namespace HRMS.Application.Features.Employees.CreateEmployee
{
    public sealed record CreateEmployeeCommand(
        string EmployeeNumber,
        string FirstName,
        string LastName,
        DateOnly DateOfBirth,
        Gender Gender,
        string NationalId,
        string Nationality,
        MaritalStatus MaritalStatus,
        string Phone,
        string Email,
        string Address,
        UploadedFile? ProfilePicture,
        int DepartmentId,
        int PositionId,
        int? ManagerEmployeeId,
        DateOnly HireDate,
        EmploymentType EmploymentType,
        EmploymentStatus EmploymentStatus,
        string WorkEmail,
        string? WorkPhone)
        : ICommand<CreateEmployeeResponse>;
}
