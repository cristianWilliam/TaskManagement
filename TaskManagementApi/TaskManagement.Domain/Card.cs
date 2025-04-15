using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain.Validators;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain;

public class Card
{
    public Guid Id { get; init; }

    public required CardDescription Description { get; init; }

    public CardStatus Status { get; private set; }

    public required CardResponsible Responsible { get; init; }

    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; private set; }

    
    // EF Constructor
    protected Card()
    {
    }
    
    public static Card CreateToDoCard(Guid id,
        CardDescription description, CardResponsible responsible,
        IDateTimeProvider dateTimeProvider)
    {
        return new Card
        {
            Id = id,
            Description = description,
            Responsible = responsible,
            CreatedOnUtc = dateTimeProvider.UtcNow,
            Status = CardStatus.Todo
        };
    }

    public Result<bool> UpdateStatus(CardStatus status,
        IDateTimeProvider dateTimeProvider)
    {
        var validationResult =
            new CanUpdateStatusSpecification().IsSatisfiedBy(this);

        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        Status = status;
        UpdatedOnUtc = dateTimeProvider.UtcNow;
        return true;
    }
}