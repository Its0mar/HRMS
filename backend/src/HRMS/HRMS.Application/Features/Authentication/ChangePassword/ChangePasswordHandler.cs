using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Authentication.ChangePassword
{
    public sealed class ChangePasswordHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ICurrentUser currentUser
        ) : ICommandHandler<ChangePasswordCommand, bool>
    {
        public async Task<ErrorOr<bool>> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(currentUser.Id, cancellationToken);
            if (user is null)
            {
                return Error.NotFound("User.NotFound", "User account not found.");
            }

            if (!passwordHasher.Verify(command.OldPassword, user.PasswordHash))
            {
                return Error.Validation("User.InvalidPassword", "The current password provided is incorrect.");
            }

            var newHash = passwordHasher.Hash(command.NewPassword);
            var isUpdated = await userRepository.ChangePasswordAsync(currentUser.Id, newHash, cancellationToken);

            if (!isUpdated)
            {
                return Error.Failure("User.PasswordUpdateFailed", "Unable to update password.");
            }

            return true;
        }
    }
}