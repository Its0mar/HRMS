using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Authentication.Logout
{
    public sealed record LogoutCommand(
        string? RefreshToken)
        : ICommand<bool>;
}
