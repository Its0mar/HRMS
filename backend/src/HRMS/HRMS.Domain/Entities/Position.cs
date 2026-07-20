namespace HRMS.Domain.Entities
{
    public class Position
    {
        public int? Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int OrganizationId { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;

        public Position(string title, string? description, int organizationId)
        {
            Title = title;
            Description = description;
            OrganizationId = organizationId;
        }

        public static Position Restore(int id, string title, int organizationId, bool IsActive, bool IsDeleted, string? description)
        {
            return new Position(title, description, organizationId)
            {
                Id = id,
                IsActive = IsActive,
                IsDeleted = IsDeleted
            };
        }
    }
}