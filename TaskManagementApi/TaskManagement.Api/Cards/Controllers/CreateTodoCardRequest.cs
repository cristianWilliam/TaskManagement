using FluentValidation;

namespace TaskManagement.Api.Cards.Controllers;

public record CreateTodoCardRequest(string Description, string Responsible);

public class AddTodoCardRequestValidator : AbstractValidator<CreateTodoCardRequest>
{
    public AddTodoCardRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.Responsible)
            .NotEmpty();
    }
}