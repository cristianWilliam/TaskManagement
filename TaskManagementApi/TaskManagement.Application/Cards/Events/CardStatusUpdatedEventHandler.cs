using MediatR;
using TaskManagement.Application.Cards.Notifications;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Cards.Events;

internal sealed class CardStatusUpdatedEventHandler : INotificationHandler<CardStatusUpdatedEvent>
{
    private readonly ICardsNotificationService _cardsNotificationService;

    public CardStatusUpdatedEventHandler(ICardsNotificationService cardsNotificationService)
    {
        _cardsNotificationService = cardsNotificationService;
    }

    public Task Handle(CardStatusUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return _cardsNotificationService.NotifyCardStatusUpdated(notification, cancellationToken);
    }
}