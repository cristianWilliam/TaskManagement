using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Domain.Validators;

internal sealed class CanUpdateStatusSpecification : ISpecification<Card>
{
    public Result<bool> IsSatisfiedBy(Card card)
    {
        return card.Status switch
        {
            CardStatus.Done => Result<bool>.Failure(
                new DomainError("Card is already in done status! No changes allowed")),
            _ => true
        };
    }
}