using System.Reflection;
using FluentValidation;

namespace TaskManagement.Api.Extensions;

public static class PresentationLayer
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}