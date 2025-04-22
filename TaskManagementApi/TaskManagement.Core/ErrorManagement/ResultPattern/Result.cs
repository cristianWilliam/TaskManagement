namespace TaskManagement.Core.ErrorManagement;

public sealed class Result<TResult> : IResult
{
    private Result(TResult value)
    {
        Value = value;
    }

    private Result(IError error)
    {
        Errors.Add(error);
    }

    private Result(IError[] errors)
    {
        Errors.AddRange(errors);
    }

    public TResult? Value { get; }

    public bool IsFailure => !IsSuccess;
    public List<IError> Errors { get; } = new();

    public bool IsSuccess => !Errors.Any();

    private static Result<TResult> Success(TResult value)
    {
        return new Result<TResult>(value);
    }

    public static Result<TResult> Failure(IError error)
    {
        return new Result<TResult>(error);
    }

    public static Result<TResult> Failure(IEnumerable<IError> errors)
    {
        return new Result<TResult>(errors.ToArray());
    }

    public static implicit operator Result<TResult>(TResult value)
    {
        return Success(value);
    }
}