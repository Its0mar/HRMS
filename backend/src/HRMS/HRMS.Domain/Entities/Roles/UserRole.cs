namespace HRMS.Domain.Entities.Roles
{
    public class UserRole
    {
        public int UserId { get; private set; }
        public int RoleId { get; private set; }
        public DateTime CreatedAt { get; private set; }
    

        public UserRole(int userId, int roleId, DateTime createdAt)
        {
            UserId = userId;
            RoleId = roleId;
            CreatedAt = createdAt;
        }
    }
}
