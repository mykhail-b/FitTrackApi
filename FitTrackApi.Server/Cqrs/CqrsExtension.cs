using FitTrackApi.Server.Cqrs.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FitTrackApi.Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        // We search for all types that implement our interfaces
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType &&
                     (i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                      i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))));

        foreach (var type in handlerTypes)
        {
            var interfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType &&
                     (i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>) ||
                      i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)));

            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, type);
            }
        }

        return services;
    }
}
