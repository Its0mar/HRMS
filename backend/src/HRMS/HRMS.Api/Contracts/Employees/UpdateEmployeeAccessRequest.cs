namespace HRMS.Api.Contracts.Employess
{
    public sealed record UpdateEmployeeAccessRequest(
        string Username,
        int RoleId);
}
