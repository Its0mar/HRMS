using HRMS.Domain.Entities.Employees.Enums;

namespace HRMS.Domain.Entities.Employees
{
    public class EmploymentInformation
    {
        public int DepartmentId { get; private set; }
        public int PositionId { get; private set; }
        public int? ManagerId { get; private set; }
        public DateOnly HireDte { get; private set;  }
        public EmploymentType EmploymentType { get; private set;  }
        public EmploymentStatus EmploymentStatus { get; private set; }
        public string WorkEmail { get; private set; }
        public string? WorkPhone { get; private set;  }

        public EmploymentInformation(
            int departmentId,
            int positionID,
            int? managerID,
            DateOnly hireDte,
            EmploymentType employmentType,
            EmploymentStatus employmentStatus,
            string workEmail,
            string? workPhone)
        {
            DepartmentId = departmentId;
            PositionId = positionID;
            ManagerId = managerID;
            HireDte = hireDte;
            EmploymentType = employmentType;
            EmploymentStatus = employmentStatus;
            WorkEmail = workEmail;
            WorkPhone = workPhone;
        }
    }
}
