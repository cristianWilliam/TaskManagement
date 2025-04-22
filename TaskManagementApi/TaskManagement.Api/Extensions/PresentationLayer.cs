using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Cards.Hubs;
using TaskManagement.Application.Cards.Notifications;
using TaskManagement.Persistence;

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
        services.AddSignalR().AddJsonProtocol(buider =>
            buider.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        
        services.AddSingleton<ICardsNotificationService, SignalRCardNotificationService>();
    }

    public static void MapWebSocketEndpoint(this WebApplication app)
    {
        app.MapHub<CardsHub>("/api/cards-hub");
    }

    public static void InitializeDbSchema(this WebApplication app)
    {
        var scope = app.Services.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        appDbContext.Database.Migrate();
    }
}