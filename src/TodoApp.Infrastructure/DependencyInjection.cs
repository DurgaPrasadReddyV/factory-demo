using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Application.Common.Interfaces;
using TodoApp.Infrastructure.Persistence;

namespace TodoApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseName = configuration["Database:Name"] ?? "TodoAppDb";

        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase(databaseName));

        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
