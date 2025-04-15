namespace TaskManagement.Core.ErrorManagement;

public sealed class Result<TResult>
{
    public Result(TResult value)
    {
        Value = value;
    }

    public Result(IError error)
    {
        Errors.Add(error);
    }

    public Result(IError[] errors)
    {
        Errors.AddRange(errors);
    }

    public TResult? Value { get; }
    public List<IError> Errors { get; } = new();

    public bool IsSuccess => Value is not null;
    public bool IsFailure => !IsSuccess;

    public static Result<TResult> Success(TResult value)
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