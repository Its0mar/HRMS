
namespace HRMS.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public int OrganizationId { get; private set; }

        public User(int id, string username, string email, string passwordHash, string firstName, string lastName, bool isActive, bool isDeleted, DateTime createdAt, DateTime? updatedAt, int organizationId)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            IsActive = isActive;
            IsDeleted = isDeleted;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            OrganizationId = organizationId;
        }

    }
}
