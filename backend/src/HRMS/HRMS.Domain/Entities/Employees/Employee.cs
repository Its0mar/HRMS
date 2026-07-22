using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities.Employees
{
    public class Employee : BaseEntity
    {
        public string EmployeeNumber { get; private set; }
        public int OrganizationId { get; private set; }
        public PersonalInformation PersonalInformation { get; private set; }
        public EmploymentInformation EmploymentInformation { get; private set; }

        public Employee(
            string employeeNumber,
            int organizationId,
            PersonalInformation personalInformation,
            EmploymentInformation employmentInformation)
        {
            EmployeeNumber = employeeNumber;
            OrganizationId = organizationId;
            PersonalInformation = personalInformation;
            EmploymentInformation = employmentInformation;
        }
    }
}
