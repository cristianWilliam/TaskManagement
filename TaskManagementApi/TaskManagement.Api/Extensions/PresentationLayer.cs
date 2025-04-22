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
        try
        {
            var scope = app.Services.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // Check if database exists and create/migrate only if needed
            if (!appDbContext.Database.CanConnect())
            {
                // Database doesn't exist or can't connect, create it
                appDbContext.Database.Migrate();
            }
            else
            {
                // Database exists, check if migrations are pending
                if (appDbContext.Database.GetPendingMigrations().Any())
                {
                    appDbContext.Database.Migrate();
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception but don't crash the application
            Console.WriteLine($"Error initializing database: {ex.Message}");
        }
    }
}
