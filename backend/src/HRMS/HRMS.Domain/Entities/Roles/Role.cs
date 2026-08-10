

namespace HRMS.Domain.Entities.Roles
{
    public class Role
    {
        public int? Id { get; private set; }
        public string Name { get; private set; }
        public int OrganizationId { get; private set; }
        private readonly List<Permission> _permissions = [];
        public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

        public Role(string name,  int organizationId, List<Permission>? permissions = null)
        {
            Name = name;
            OrganizationId = organizationId;
            if (permissions is not null) _permissions.AddRange(permissions);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public static Role Restore(int id, string name,  int organizationId, List<Permission> permissions)
        {
            return new Role(name, organizationId, permissions)
            {
                Id = id,
            };
        }

        
    }
}
