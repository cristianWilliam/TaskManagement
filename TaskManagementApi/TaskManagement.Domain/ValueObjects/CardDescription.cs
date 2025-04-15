using FluentValidation;
using FluentValidation.Results;

using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.ValueObjects;

public sealed class CardDescription : IValueObject<CardDescription>
{
    // EF Constructor
    private CardDescription() { }
    public string? Value { get; init; }

    public static Result<CardDescription> Create(string value)
    {
        CardDescription instance = new() {Value = value};

        var validationResult = new Validator().Validate(instance);
        if (validationResult.IsValid)
        {
            return instance;
        }

        return validationResult.ToValidationErrorsResult<CardDescription>();
    }

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