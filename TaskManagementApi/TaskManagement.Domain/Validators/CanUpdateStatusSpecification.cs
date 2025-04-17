using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.Validators;

internal sealed class CanUpdateStatusSpecification : ISpecification<Card>
{
    public Result<bool> IsSatisfiedBy(Card card) =>
        card.Status switch
        {
            CardStatus.Done => Result<bool>.Failure(new DomainError("Card is already in done status! No changes allowed")),
            _ => true,
        };
}