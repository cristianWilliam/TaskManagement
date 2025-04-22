using System.Reflection;
using FluentValidation;
using TaskManagement.Api.Cards.Hubs;
using TaskManagement.Application.Cards.Notifications;

namespace TaskManagement.Api.Extensions;

public static class PresentationLayer
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        // Fluent Validations
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // WebSocket
        services.AddSignalRServices();

        return services;
    }

    private static void AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<ICardsNotificationService, SignalRCardNotificationService>();
    }

    public static void MapWebSocketEndpoint(this WebApplication app)
    {
        app.MapHub<CardsHub>("/api/cards-hub");
    }
}