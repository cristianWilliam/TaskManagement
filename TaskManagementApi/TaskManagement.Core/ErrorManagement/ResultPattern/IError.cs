namespace TaskManagement.Core.ErrorManagement.ResultPattern;

public interface IError
{
    string Error { get; init; }
    ErrorType Type { get; init; }
}