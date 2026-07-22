using HRMS.Domain.Entities.Employees.Enums;

namespace HRMS.Domain.Entities.Employees
{
    public sealed class EmploymentInformation
    {
        public int DepartmentId { get; private set; }
        public int PositionId { get; private set; }
        public int? ManagerEmployeeId { get; private set; }
        public DateOnly HireDate { get; private set;  }
        public EmploymentType EmploymentType { get; private set;  }
        public EmploymentStatus EmploymentStatus { get; private set; }
        public string WorkEmail { get; private set; }
        public string? WorkPhone { get; private set;  }

        public EmploymentInformation(
            int departmentId,
            int positionId,
            int? managerEmployeeId,
            DateOnly hireDate,
            EmploymentType employmentType,
            EmploymentStatus employmentStatus,
            string workEmail,
            string? workPhone)
        {
            DepartmentId = departmentId;
            PositionId = positionId;
            ManagerEmployeeId = managerEmployeeId;
            HireDate = hireDate;
            EmploymentType = employmentType;
            EmploymentStatus = employmentStatus;
            WorkEmail = workEmail;
            WorkPhone = workPhone;
        }

        public void ChangeAssignment(
            int departmentId,
            int positionId,
            int? managerId)
        {
            DepartmentId = departmentId;
            PositionId = positionId;
            ManagerEmployeeId = managerId;
        }
    }
}
