using HRMS.Application.Abstractions.Messaging;
using System.Windows.Input;

namespace HRMS.Application.Features.Authentication.Login
{
    public record LoginCommand(
        string Identifier,
        string Password
        ) : ICommand<LoginResponse>;
}
