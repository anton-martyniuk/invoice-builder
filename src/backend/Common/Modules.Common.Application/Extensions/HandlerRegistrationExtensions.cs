using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Domain.Events;
using Modules.Common.Domain.Handlers;

namespace Modules.Common.Application.Extensions;

public static class HandlerRegistrationExtensions
{
    /// <summary>
    /// Registers all handlers from the assembly containing the specified type
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="marker">A type from the assembly where handlers are located</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection RegisterHandlersFromAssemblyContaining(this IServiceCollection services, Type marker)
    {
        var assembly = marker.Assembly;

        // Register command handlers (IHandler implementations)
        RegisterCommandHandlers(services, assembly);

        // Register event handlers (IEventHandler implementations)
        RegisterEventHandlers(services, assembly);

        return services;
    }

    private static void RegisterCommandHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                && t.IsAssignableTo(typeof(IHandler))
                && !t.IsAssignableTo(typeof(IEventHandler)))
            .ToList();

        foreach (var implementationType in handlerTypes)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IHandler) && i.IsAssignableTo(typeof(IHandler)));

            if (interfaceType is not null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }

    private static void RegisterEventHandlers(IServiceCollection services, Assembly assembly)
    {
        var eventHandlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                && t.IsAssignableTo(typeof(IEventHandler))
                && !t.IsAssignableTo(typeof(IHandler)))
            .ToList();

        foreach (var implementationType in eventHandlerTypes)
        {
            // Find all IEventHandler<T> interfaces implemented by this type
            var handlerInterfaces = implementationType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

            foreach (var interfaceType in handlerInterfaces)
            {
                // Register as both the specific IEventHandler<T> and the non-generic IEventHandler
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}
