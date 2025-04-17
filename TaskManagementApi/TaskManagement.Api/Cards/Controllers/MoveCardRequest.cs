using FluentValidation;
using TaskManagement.Domain;

namespace TaskManagement.Api.Cards.Controllers;

public record MoveCardRequest(Guid? CardId, CardStatus NewStatus);

public class MoveCardRequestValidator : AbstractValidator<MoveCardRequest>
{
    public MoveCardRequestValidator()
    {
        RuleFor(x => x.CardId)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}