
namespace HRMS.Domain.Entities.Roles
{
    public class Role
    {
        public int? Id { get; private set; }
        public string Name { get; private set; }
        public int OrganizationId { get; private set; }

        public Role(string name,  int organizationId)
        {
            Name = name;
            OrganizationId = organizationId;
        }

        public static Role Restore(int id, string name,  int organizationId)
        {
            return new Role(name, organizationId)
            {
                Id = id,
            };
        }
    }
}
