using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Behaviors;

namespace TodoApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);

            // Order matters: logging wraps validation, so a rejected request is still logged.
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
