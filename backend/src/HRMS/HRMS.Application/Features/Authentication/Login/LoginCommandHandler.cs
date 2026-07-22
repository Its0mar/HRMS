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
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _tokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IValidator<LoginCommand> _validator;

        public LoginCommandHandler(
            IUserRepository users,
            IRefreshTokenRepository refreshTokenrepository,
            IPasswordHasher passwordHasher,
            IAccessTokenGenerator tokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator,
            IValidator<LoginCommand> validator)
        {
            _userRepository = users;
            _refreshTokenRepository = refreshTokenrepository; 
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
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

            var user = await _userRepository.GetByIdentifierAsync(
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

            var userId = user.Id ?? throw new ArgumentNullException("userId can`t be null");
            
            var refreshToken = _refreshTokenGenerator.Generate();
            await _refreshTokenRepository.RemoveForUserAsync(userId, cancellationToken);
            await _refreshTokenRepository.CreateRefreshTokenAsync(userId, refreshToken, DateTime.UtcNow.AddDays(7), DateTime.UtcNow, cancellationToken);
            

            return new LoginResponse(
                AuthenticatedUserResponse.From(user),
                _tokenGenerator.Generate(user),
                refreshToken);
        }
    }
}
