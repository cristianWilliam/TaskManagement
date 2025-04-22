using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Core.ErrorManagement;

public sealed record DomainError(string Error) : IError
{
    public ErrorType Type { get; init; } = ErrorType.DomainError;
}