using FluentValidation;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Domain.ValueObjects;

public sealed class CardDescription
{
    // EF Constructor
    private CardDescription()
    {
    }

    public string? Value { get; init; }

    public static Result<CardDescription> Create(string value)
    {
        var instance = new CardDescription {Value = value};

        var validationResult = new Validator().Validate(instance);
        return validationResult.IsValid ? instance : validationResult.ToValidationErrorsResult<CardDescription>();
    }

    // Castings
    public override string ToString()
    {
        return Value ?? string.Empty;
    }

    public static implicit operator string(CardDescription value)
    {
        return value.ToString();
    }

    // Validator Class
    private class Validator : AbstractValidator<CardDescription>
    {
        public Validator()
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}