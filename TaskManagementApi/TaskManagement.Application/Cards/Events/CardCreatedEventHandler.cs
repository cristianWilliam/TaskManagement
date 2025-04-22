using MediatR;
using TaskManagement.Domain.Events;

namespace TaskManagement.Application.Cards.Events;

internal sealed class CardCreatedEventHandler : INotificationHandler<CardCreatedEvent>
{
    public Task Handle(CardCreatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}