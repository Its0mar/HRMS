namespace HRMS.Application.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        public int Id { get; }
        public int OrganizationId { get; }
        public int EmployeeId { get; }
        public bool IsAuthenticated { get; }
    }
}
