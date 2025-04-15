using FluentValidation;
using FluentValidation.Results;

using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.ValueObjects;

public sealed class CardResponsible : IValueObject<CardResponsible>
{
    // Ef Constructor
    private CardResponsible() { }
    public string? Value { get; init; }

    public static Result<CardResponsible> Create(string value)
    {
        CardResponsible instance = new() {Value = value};

        var validationResult = new Validator().Validate(instance);
        if (validationResult.IsValid)
        {
            return instance;
        }

        return validationResult.ToValidationErrorsResult<CardResponsible>();
    }

    private class Validator : AbstractValidator<CardResponsible>
    {
        public Validator()
        {
            RuleFor(x => x.Value)
                .NotEmpty();
        }
    }
}