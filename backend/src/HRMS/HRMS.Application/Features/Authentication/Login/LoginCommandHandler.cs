using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Authentication;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Abstractions.Persistence;


namespace HRMS.Application.Features.Authentication.Login
{
    internal sealed class LoginCommandHandler
    : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _tokenGenerator;
        private readonly IValidator<LoginCommand> _validator;

        public LoginCommandHandler(
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IAccessTokenGenerator tokenGenerator,
            IValidator<LoginCommand> validator)
        {
            _users = users;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _validator = validator;
        }

        public async Task<ErrorOr<LoginResponse>> HandleAsync(
            LoginCommand command,
            CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(
                command,
                cancellationToken);

            if (!validation.IsValid)
            {
                return validation.Errors
                    .Select(failure => Error.Validation(
                        code: $"Login.{failure.PropertyName}",
                        description: failure.ErrorMessage))
                    .ToList();
            }

            var identifier = command.Identifier.Trim();

            if (identifier.Contains('@'))
            {
                identifier = identifier.ToLowerInvariant();
            }

            var user = await _users.GetByIdentifierAsync(
                identifier,
                cancellationToken);

            if (user is null ||
                user.IsDeleted ||
                !user.IsActive ||
                !_passwordHasher.Verify(
                    command.Password,
                    user.PasswordHash))
            {
                return AuthenticationErrors.InvalidCredentials;
            }

            return new LoginResponse(
                AuthenticatedUserResponse.From(user),
                _tokenGenerator.Generate(user));
        }
    }
}
