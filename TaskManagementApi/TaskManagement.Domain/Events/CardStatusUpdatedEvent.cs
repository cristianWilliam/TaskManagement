namespace TaskManagement.Domain.Events;

public record CardStatusUpdatedEvent(
    Guid CardId, CardStatus OldStatus, CardStatus NewStatus): IDomainEvent;