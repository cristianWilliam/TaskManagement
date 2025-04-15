using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.Validators;

internal sealed class CanUpdateStatusSpecification : ISpecification<Card>
{
    public Result<bool> IsSatisfiedBy(Card card)
    {
        if (card.Status == CardStatus.Removed)
        {
            return Result<bool>.Failure(
                new DomainError("Card is Removed! No changes allowed"));
        }

        if (card.Status == CardStatus.Done)
        {
            return Result<bool>.Failure(
                new DomainError("Card is already done! No changes allowed"));
        }

        return true;
    }
}