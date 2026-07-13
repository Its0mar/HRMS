
namespace HRMS.Domain.Entities
{
    public class Organization
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set;  }
        public string Email { get; private set; }
        public string Address { get; private set; }
        public string Website { get; private set; }
        public string LogoUrl { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public int CreatedById { get; private set; }
        public int? UpdatedById { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }



        public Organization(int id, string name, string code, string email, string address, string website, string logoUrl, bool isActive, bool isDeleted, int createdById, int? updatedById, DateTime createdAt, DateTime? updatedAt)
        {
            Id = id;
            Name = name;
            Code = code;
            Email = email;
            Address = address;
            Website = website;
            LogoUrl = logoUrl;
            IsActive = isActive;
            IsDeleted = isDeleted;
            CreatedById = createdById;
            UpdatedById = updatedById;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
