using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

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

            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            ErrorOr<TResponse> result = await handler.HandleAsync(
                (dynamic)command,
                cancellationToken);

            //Automatic Cache Eviction upon successful command execution
            if (!result.IsError && command is ICacheEvictingCommand evictingCommand)
            {
                var cacheService = _serviceProvider.GetService<ICacheService>();
                if (!string.IsNullOrWhiteSpace(evictingCommand.CacheKeyToEvict))
                {
                    cacheService?.Remove(evictingCommand.CacheKeyToEvict);
                }
            }

            return result;
        }
    }
}
