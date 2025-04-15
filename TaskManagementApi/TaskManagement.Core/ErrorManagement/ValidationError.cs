namespace TaskManagement.Core.ErrorManagement;

public record ValidationError(string Error) : IError
{
    public ErrorType Type { get; init; } = ErrorType.ValidationError;
}