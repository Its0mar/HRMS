using System.Reflection;
using FluentValidation;
using HRMS.Application.Abstractions.Messaging;
using HRMS.Application.Features.Organizations.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace HRMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Auto-register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<RegisterOrganizationCommandValidator>();

        // Core CQRS Dispatchers
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();

        // Auto-register all Command and Query handlers dynamically via reflection
        RegisterGenericHandlers(services, assembly, typeof(ICommandHandler<,>));
        RegisterGenericHandlers(services, assembly, typeof(IQueryHandler<,>));

        return services;
    }

    private static void RegisterGenericHandlers(IServiceCollection services, Assembly assembly, Type genericInterfaceType)
    {
        var handlerMappings = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (implementation, serviceInterface) => new { Implementation = implementation, Interface = serviceInterface })
            .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == genericInterfaceType);

        foreach (var mapping in handlerMappings)
        {
            services.AddScoped(mapping.Interface, mapping.Implementation);
        }
    }
}
