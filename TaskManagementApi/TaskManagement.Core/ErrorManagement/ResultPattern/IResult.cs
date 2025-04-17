namespace TaskManagement.Core.ErrorManagement;

public interface IResult
{
    List<IError> Errors { get; }
    bool IsSuccess { get; }
}