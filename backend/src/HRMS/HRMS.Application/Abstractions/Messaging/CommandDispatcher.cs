using System.Diagnostics;
using ErrorOr;
using FluentValidation;
using HRMS.Application.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Abstractions.Messaging
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandDispatcher> _logger;

        public CommandDispatcher(IServiceProvider serviceProvider, ILogger<CommandDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<ErrorOr<TResponse>> SendAsync<TResponse>(
            ICommand<TResponse> command,
            CancellationToken cancellationToken)
        {
            var commandType = command.GetType();
            var commandName = commandType.Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Executing Command {CommandName}", commandName);

            var validatorType = typeof(IValidator<>).MakeGenericType(commandType);
            var validators = _serviceProvider.GetServices(validatorType).Cast<IValidator>().ToList();

            if (validators.Count > 0)
            {
                var context = new ValidationContext<object>(command);
                var validationResults = await Task.WhenAll(
                    validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

                var errors = validationResults
                    .SelectMany(result => result.Errors)
                    .Where(failure => failure is not null)
                    .Select(failure => Error.Validation(
                        code: $"{commandName}.{failure.PropertyName}",
                        description: failure.ErrorMessage))
                    .ToList();

                if (errors.Count > 0)
                {
                    _logger.LogWarning("Command {CommandName} failed validation with {ErrorCount} errors", commandName, errors.Count);
                    return errors;
                }
            }

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));
            dynamic handler = _serviceProvider.GetRequiredService(handlerType);

            ErrorOr<TResponse> result = await handler.HandleAsync((dynamic)command, cancellationToken);

            stopwatch.Stop();

            if (result.IsError)
            {
                _logger.LogWarning("Command {CommandName} completed with errors in {ElapsedMs}ms", commandName, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation("Command {CommandName} completed successfully in {ElapsedMs}ms", commandName, stopwatch.ElapsedMilliseconds);
            }

            // Automatic Cache Eviction upon successful command execution
            if (!result.IsError && command is ICacheEvictingCommand evictingCommand)
            {
                var cacheService = _serviceProvider.GetService<ICacheService>();
                if (!string.IsNullOrWhiteSpace(evictingCommand.CacheKeyToEvict))
                {
                    _logger.LogInformation("Evicting cache key {CacheKey} for command {CommandName}", evictingCommand.CacheKeyToEvict, commandName);
                    cacheService?.Remove(evictingCommand.CacheKeyToEvict);
                }
            }

            return result;
        }
    }
}
