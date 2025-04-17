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
    public List<IError> Errors { get; } = new List<IError>();

    public bool IsSuccess => !Errors.Any();

    public bool IsFailure => !IsSuccess;

    private static Result<TResult> Success(TResult value) => new Result<TResult>(value);

    public static Result<TResult> Failure(IError error) => new Result<TResult>(error);

    public static Result<TResult> Failure(IEnumerable<IError> errors) => new Result<TResult>(errors.ToArray());

    public static implicit operator Result<TResult>(TResult value) => Success(value);
}