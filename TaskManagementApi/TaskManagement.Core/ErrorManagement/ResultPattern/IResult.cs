namespace TaskManagement.Core.ErrorManagement.ResultPattern;

public interface IResult
{
    List<IError> Errors { get; }
    bool IsSuccess { get; }
}