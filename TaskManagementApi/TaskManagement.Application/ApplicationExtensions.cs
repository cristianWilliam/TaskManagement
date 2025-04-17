using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Providers;
using TaskManagement.Domain;

namespace TaskManagement.Application;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configure =>
            configure.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()));

        services.AddSingleton<IGuidProvider, GuidProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}