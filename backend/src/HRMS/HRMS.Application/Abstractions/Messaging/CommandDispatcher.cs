using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace HRMS.Application.Abstractions.Messaging
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ErrorOr<TResponse>> SendAsync<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken)
        {
            var commandType = command.GetType();

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(commandType);

            var validators = _serviceProvider
                .GetServices(validatorType)
                .Cast<IValidator>()
                .ToList();

            if (validators.Count > 0)
            {
                var context = new ValidationContext<object>(command);

                var validationResults = await Task.WhenAll(
                    validators.Select(validator =>
                        validator.ValidateAsync(
                            context,
                            cancellationToken)));

                var errors = validationResults
                    .SelectMany(result => result.Errors)
                    .Where(failure => failure is not null)
                    .Select(failure => Error.Validation(
                        code: $"{commandType.Name}.{failure.PropertyName}",
                        description: failure.ErrorMessage))
                    .ToList();

                if (errors.Count > 0)
                {
                    return errors;
                }
            }

            var handlerType = typeof(ICommandHandler<,>)
                .MakeGenericType(commandType, typeof(TResponse));

            dynamic handler =
                _serviceProvider.GetRequiredService(handlerType);

            return await handler.HandleAsync(
                (dynamic)command,
                cancellationToken);
        }
    }
}
