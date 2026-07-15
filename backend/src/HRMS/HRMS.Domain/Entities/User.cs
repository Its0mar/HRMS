using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int OrganizationId { get; private set; }

        public User(
            string username,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            int organizationId)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            OrganizationId = organizationId;
        }


        public static User Restore(
            int id,
            string username,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            int organizationId,
            bool isActive,
            bool isDeleted,
            DateTime createdAt,
            DateTime? updatedAt)
        {
            var user = new User(
                username,
                email,
                passwordHash,
                firstName,
                lastName,
                organizationId)
            {
                Id = id,
                IsActive = isActive
            };
            user.IsDeleted = isDeleted;
            user.CreatedAt = createdAt;
            user.UpdatedAt = updatedAt;

            return user;
        }

    }
}
