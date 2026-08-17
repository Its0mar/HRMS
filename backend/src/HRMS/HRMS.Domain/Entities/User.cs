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
        public int? EmployeeId { get; private set; } = null;
        public bool CanAuthenticate => !IsDeleted && IsActive;

        public User(
            string username,
            string email,
            string passwordHash,
            string firstName,
            string lastName,
            int organizationId,
            int? employeeId = null)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            LastName = lastName;
            OrganizationId = organizationId;
            EmployeeId = employeeId;
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
            DateTime? updatedAt,
            int? employeeId = null)
        {
            var user = new User(
                username,
                email,
                passwordHash,
                firstName,
                lastName,
                organizationId,
                employeeId)
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
