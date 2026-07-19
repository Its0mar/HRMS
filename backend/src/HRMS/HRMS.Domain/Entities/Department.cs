using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string? Description { get; private set; }
        public int? ManagerEmployeeId { get; private set; }
        public int OrganizationId { get; private set; }


        public Department(string name, string code, int organizationId, string? description, int? managerEmployeeId)
        {
            Name = name;
            Code = code;
            Description = description;
            ManagerEmployeeId = managerEmployeeId;
            OrganizationId = organizationId;
        }
    }
}
