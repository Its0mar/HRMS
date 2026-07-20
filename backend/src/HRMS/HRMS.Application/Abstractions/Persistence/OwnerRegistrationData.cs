namespace HRMS.Application.Abstractions.Persistence
{
    public sealed record OwnerRegistrationData(
        string Username,
        string Email,
        string PasswordHash,
        string FirstName,
        string LastName);
}
