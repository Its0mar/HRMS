using ErrorOr;
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

        public CreatePositionCommandHandler(
            IPositionsRepository positionsRepository,
            ICurrentUser currentUser)
        {
            _positionsRepository = positionsRepository;
            _currentUser = currentUser;
        }

        public async Task<ErrorOr<int>> HandleAsync(CreatePositionCommand command, CancellationToken cancellationToken)
        {
            string title = command.Title.Trim();
            var description = command.Description is null ? null : command.Description.Trim();

            var titleExist =  await _positionsRepository.TitleExistsAsync(_currentUser.OrganizationId, command.Title, cancellationToken);
            if (titleExist)
            {
                return PositionsErrors.TitleExists;
            }

            var position = new Position(title, description, _currentUser.OrganizationId);
            var result = await _positionsRepository.CreateAsync(position, cancellationToken);

            return result;
        }
    }
}
