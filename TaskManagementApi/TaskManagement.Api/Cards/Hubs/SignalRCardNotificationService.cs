using Microsoft.AspNetCore.SignalR;
using TaskManagement.Application.Cards.Notifications;
using TaskManagement.Domain.Events;

namespace TaskManagement.Api.Cards.Hubs;

internal sealed class SignalRCardNotificationService : ICardsNotificationService
{
    private readonly IHubContext<CardsHub> _hubContext;

    public SignalRCardNotificationService(IHubContext<CardsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyCardStatusUpdated(CardStatusUpdatedEvent cardStatusUpdatedEvent,
        CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync(CardsHub.CardUpdated, cardStatusUpdatedEvent, cancellationToken);
    }

    public Task NotifyCardCreated(CardCreatedEvent cardCreatedEvent,
        CancellationToken cancellationToken)
    {
        return _hubContext.Clients.All.SendAsync(CardsHub.CardCreated, cardCreatedEvent, cancellationToken);
    }
}