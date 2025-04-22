using MediatR;
using TaskManagement.Application.Cards.Notifications;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Cards.Events;

internal sealed class CardCreatedEventHandler : INotificationHandler<CardCreatedEvent>
{
    private readonly ICardsNotificationService _cardsNotificationService;

    public CardCreatedEventHandler(ICardsNotificationService cardsNotificationService)
    {
        _cardsNotificationService = cardsNotificationService;
    }

    public Task Handle(CardCreatedEvent notification, CancellationToken cancellationToken)
    {
        return _cardsNotificationService.NotifyCardCreated(notification, cancellationToken);
    }
}