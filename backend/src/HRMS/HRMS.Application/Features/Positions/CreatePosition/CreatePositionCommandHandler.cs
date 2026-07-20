using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;
using HRMS.Domain.Entities;

namespace HRMS.Application.Features.Positions.CreatePosition
{
    public class CreatePositionCommandHandler
        : ICommandHandler<CreatePositionCommand, int>
    {
        private readonly IPositionsRepository _positionsRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IValidator<CreatePositionCommand> _validator;

        public CreatePositionCommandHandler(
            IPositionsRepository positionsRepository,
            ICurrentUser currentUser,
            IValidator<CreatePositionCommand> validator)
        {
            _positionsRepository = positionsRepository;
            _currentUser = currentUser;
            _validator = validator;
        }

        public async Task<ErrorOr<int>> HandleAsync(CreatePositionCommand command, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(command, cancellationToken);

            if (!validation.IsValid)
            {
                return validation.Errors
                    .Select(failure => Error.Validation(
                        code: $"UpdateDepartment.{failure.PropertyName}",
                        description: failure.ErrorMessage))
                    .ToList();
            }

            string title = command.Title.Trim();
            var description = command.Description is null ? null : command.Description.Trim();

            var titleExist =  await _positionsRepository.TitleExistsAsync(_currentUser.OrganizationId, command.Title, cancellationToken);
            if (titleExist)
            {
                return Error.Conflict("a position with this title already exist");
            }

            var position = new Position(title, description, _currentUser.OrganizationId);
            var result = await _positionsRepository.CreateAsync(position, cancellationToken);

            return result;
        }
    }
}
