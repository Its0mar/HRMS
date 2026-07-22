namespace HRMS.Application.Abstractions.Authentication
{
    public interface IRefreshTokenGenerator
    {
        string Generate();
    }
}
