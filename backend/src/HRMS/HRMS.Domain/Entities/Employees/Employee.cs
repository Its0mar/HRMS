using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities.Employees
{
    public sealed class Employee : BaseEntity
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
            if (string.IsNullOrWhiteSpace(employeeNumber))
                throw new ArgumentException("Employee number is required.");

            if (organizationId <= 0)
                throw new ArgumentOutOfRangeException(nameof(organizationId));

            ArgumentNullException.ThrowIfNull(personalInformation);
            ArgumentNullException.ThrowIfNull(employmentInformation);


            EmployeeNumber = employeeNumber;
            OrganizationId = organizationId;
            PersonalInformation = personalInformation;
            EmploymentInformation = employmentInformation;
        }

        public static Employee Restore(
            int id,
            string employeeNumber,
            int organizationId,
            PersonalInformation personalInformation,
            EmploymentInformation employmentInformation,
            bool isDeleted,
            bool isActive,
            DateTime createdAt,
            DateTime? updatedAt)
        {
            return new Employee(
                employeeNumber,
                organizationId,
                personalInformation,
                employmentInformation)
            {
                Id = id,
                IsDeleted = isDeleted,
                IsActive = isActive,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };
        }


        public void UpdatePersonalInformation(PersonalInformation information)
        {
            PersonalInformation = information;
        }

        public void UpdateEmploymentInformation(EmploymentInformation information)
        {
            EmploymentInformation = information;
        }
    }
}
