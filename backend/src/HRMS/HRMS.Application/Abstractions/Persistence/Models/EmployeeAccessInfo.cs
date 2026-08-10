
namespace HRMS.Application.Abstractions.Persistence.Models
{
    public record EmployeeAccessInfo(
        int UserId,
        int EmployeeId,
        int RoleId,
        string UserName);
}
