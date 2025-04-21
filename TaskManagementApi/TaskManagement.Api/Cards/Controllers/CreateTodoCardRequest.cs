using FluentValidation;

namespace TaskManagement.Api.Cards.Controllers;

public record CreateTodoCardRequest(string Description, string Responsible);

public class CreateTodoCardRequestValidator : AbstractValidator<CreateTodoCardRequest>
{
    public CreateTodoCardRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.Responsible)
            .NotEmpty();
    }
}