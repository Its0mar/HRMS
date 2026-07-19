using HRMS.Domain.Entities;

namespace HRMS.Application.Abstractions.Authentication;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
