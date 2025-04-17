using MediatR;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Application.Cards.Create;

public record CreateTodoCardCommand(
    CardDescription Description,
    CardResponsible Responsible) : IRequest<Result<CardDto>>
{
    public static Result<CreateTodoCardCommand> TryCreate(string description, string responsible)
    {
        var descriptionResult = CardDescription.Create(description);
        var responsibleResult = CardResponsible.Create(responsible);

        return ResultHelpers.MergeResults(
            () => new CreateTodoCardCommand(descriptionResult.Value!, responsibleResult.Value!),
            descriptionResult, responsibleResult);
    }
};