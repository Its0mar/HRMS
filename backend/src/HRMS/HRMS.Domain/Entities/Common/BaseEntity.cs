
namespace HRMS.Domain.Entities.Common
{
    public abstract class BaseEntity
    {
        public int? Id { get; protected set; }

        public bool IsDeleted { get; protected set; }
        public bool IsActive { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        public DateTime? UpdatedAt { get; protected set; }


        public BaseEntity()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
