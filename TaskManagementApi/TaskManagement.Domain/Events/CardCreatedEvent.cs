namespace TaskManagement.Domain.Events;

public record CardCreatedEvent(
    Guid CardId,
    string Description,
    string Responsible,
    DateTime CreationDateUtc,
    CardStatus Status)
    : IDomainEvent;