using ErrorOr;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;

namespace HRMS.Application.Features.Employees.UpdateEmployeeAccess
{
    public class UpdateEmployeeAccessHandler : ICommandHandler<UpdateEmployeeAccessCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public UpdateEmployeeAccessHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<bool>> HandleAsync(UpdateEmployeeAccessCommand command, CancellationToken cancellationToken)
        {
            var resutl = await _userRepository.UpdateAccessAsync(command.EmployeeId, _currentUser.OrganizationId, command.Username, command.RoleId, cancellationToken);

            if (!resutl)
            {
                return Error.Failure(description: "access update failed");
            }

            return true;
        }
    }
}
