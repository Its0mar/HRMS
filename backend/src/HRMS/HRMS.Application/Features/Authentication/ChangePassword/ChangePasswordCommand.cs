using HRMS.Application.Abstractions.Messaging;

namespace HRMS.Application.Features.Authentication.ChangePassword
{
    public record ChangePasswordCommand(string OldPassword, string NewPassword) : ICommand<bool>;
}
