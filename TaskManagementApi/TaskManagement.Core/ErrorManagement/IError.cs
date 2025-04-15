namespace TaskManagement.Core.ErrorManagement;

public interface IError
{
    string Error { get; init; }
    ErrorType Type { get; init; }
}