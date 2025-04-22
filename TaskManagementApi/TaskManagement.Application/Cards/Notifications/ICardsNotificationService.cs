using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Cards.Notifications;

public interface ICardsNotificationService
{
    Task NotifyCardStatusUpdated(CardStatusUpdatedEvent cardStatusUpdatedEvent, CancellationToken cancellationToken);
    Task NotifyCardCreated(CardCreatedEvent cardCreatedEvent, CancellationToken cancellationToken);
}