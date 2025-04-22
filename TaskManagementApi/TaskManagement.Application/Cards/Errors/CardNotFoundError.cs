using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Application.Cards.Errors;

public record CardNotFoundError(Guid CardId) : IError
{
    public string Error { get; init; } = $"Card [{CardId}] not found";
    public ErrorType Type { get; init; } = ErrorType.NotFoundError;
}