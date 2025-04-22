using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManagement.Domain;

namespace TaskManagement.Persistence.Interceptors;

internal sealed class PublishDomainInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public PublishDomainInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new())
    {
        var cardsEntry = eventData.Context?.ChangeTracker.Entries<Card>().ToArray();
        var domainEvents = cardsEntry?.SelectMany(x => x.Entity.DomainEvents).ToArray();

        if (cardsEntry is null || !cardsEntry.Any() || domainEvents is null || !domainEvents.Any())
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        
        foreach (var entityEntry in cardsEntry)
            entityEntry.Entity.ClearEvents();
        
        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
        
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}