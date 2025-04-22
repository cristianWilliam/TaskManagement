namespace TaskManagement.Domain.Events;

public abstract class BaseDomainEvents
{
    protected List<IDomainEvent> _domainEvents { get; } = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}