using HRMS.Domain.Entities.Common;

namespace HRMS.Domain.Entities;

public sealed class Organization : BaseEntity
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Email { get; private set; }
    public string? Address { get; private set; }
    public string? Website { get; private set; }
    public string? LogoUrl { get; private set; }
    public int? UpdatedById { get; private set; }

    public Organization(
        string name,
        string code,
        string email,
        string? address = null,
        string? website = null,
        string? logoUrl = null)
    {
        Name = name;
        Code = code;
        Email = email;
        Address = address;
        Website = website;
        LogoUrl = logoUrl;
    }

    public static Organization Restore(
        int id,
        string name,
        string code,
        string email,
        bool isActive,
        bool isDeleted,
        DateTime createdAt,
        string? address,
        string? website,
        string? logoUrl,
        int? updatedById,
        DateTime? updatedAt)
    {
        return new Organization(
            name,
            code,
            email,
            address,
            website,
            logoUrl)
        {
            Id = id,
            IsActive = isActive,
            IsDeleted = isDeleted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            UpdatedById = updatedById
        };
    }

    public void UpdateDetails(
        string name,
        string email,
        string? address,
        string? website,
        string? logoUrl,
        int updatedById)
    {
        Name = name;
        Email = email;
        Address = address;
        Website = website;
        LogoUrl = logoUrl;
        UpdatedById = updatedById;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}