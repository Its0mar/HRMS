using HRMS.Domain.Entities.Employees.Enums;

namespace HRMS.Api.Contracts.Employees
{
    public sealed class CreateEmployeeRequest
    {
        public string EmployeeNumber { get; init; } = string.Empty;

        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public DateOnly DateOfBirth { get; init; }

        public Gender Gender { get; init; }

        public string NationalId { get; init; } = string.Empty;
        public string Nationality { get; init; } = string.Empty;

        public MaritalStatus MaritalStatus { get; init; }

        public string Phone { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;

        public IFormFile? ProfilePicture { get; init; }

        public int DepartmentId { get; init; }
        public int PositionId { get; init; }
        public int? ManagerEmployeeId { get; init; }

        public DateOnly HireDate { get; init; }

        public EmploymentType EmploymentType { get; init; }
        public EmploymentStatus EmploymentStatus { get; init; }

        public string WorkEmail { get; init; } = string.Empty;
        public string? WorkPhone { get; init; }
    }
}
