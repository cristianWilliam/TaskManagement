namespace TaskManagement.Core.ErrorManagement.ResultPattern;

public static class ResultExtensions
{
    public static TResponse Match<TResponse, TResult>(this Result<TResult> result, Func<IError[], TResponse> onErrorHandler,
        Func<TResult, TResponse> onSuccessHandler)
    {
        return result.IsFailure ? onErrorHandler(result.Errors.ToArray()) : onSuccessHandler(result.Value!);
    }
}