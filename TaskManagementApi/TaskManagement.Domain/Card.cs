using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;
using TaskManagement.Domain.Events;
using TaskManagement.Domain.Validators;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain;

public class Card : BaseDomainEvents
{
    // EF Constructor
    protected Card()
    {
    }

    public Guid Id { get; init; }

    public required CardDescription Description { get; init; }

    public CardStatus Status { get; private set; }

    public required CardResponsible Responsible { get; init; }

    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; private set; }

    public static Result<Card> CreateToDoCard(Guid id,
        CardDescription description, CardResponsible responsible,
        IDateTimeProvider dateTimeProvider)
    {
        var newCard = new Card
        {
            Id = id,
            Description = description,
            Responsible = responsible,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            Status = CardStatus.Todo
        };

        newCard.AddDomainEvent(
            new CardCreatedEvent(
                newCard.Id, description.Value!, responsible.Value!,
                newCard.CreatedOnUtc, newCard.Status));

        return newCard;
    }

    public Result<bool> UpdateStatus(CardStatus newStatus, IDateTimeProvider dateTimeProvider)
    {
        var validationResult = new CanUpdateStatusSpecification().IsSatisfiedBy(this);

        if (validationResult.IsFailure)
            return validationResult;

        AddDomainEvent(new CardStatusUpdatedEvent(Id, Status, newStatus));

        Status = newStatus;
        UpdatedOnUtc = dateTimeProvider.UtcNow;
        return true;
    }
}