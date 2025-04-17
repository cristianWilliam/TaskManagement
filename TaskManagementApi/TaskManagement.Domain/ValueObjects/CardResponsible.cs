using FluentValidation;
using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.ValueObjects;

public sealed class CardResponsible
{
    // Ef Constructor
    private CardResponsible()
    {
    }

    public string? Value { get; init; }

    public static Result<CardResponsible> Create(string value)
    {
        var instance = new CardResponsible { Value = value };

        var validationResult = new Validator().Validate(instance);
        return validationResult.IsValid
            ? instance
            : validationResult.ToValidationErrorsResult<CardResponsible>();
    }

    // Validator Class
    private class Validator : AbstractValidator<CardResponsible>
    {
        public Validator()
        {
            this.RuleFor(x => x.Value)
                .NotEmpty();
        }
    }

    // Castings
    public override string ToString() => Value ?? string.Empty;
    public static implicit operator string(CardResponsible value) => value.ToString();
}