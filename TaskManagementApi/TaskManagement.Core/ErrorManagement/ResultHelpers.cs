using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Core.ErrorManagement;

public static class ResultHelpers
{
    public static Result<T> MergeResults<T>(Func<T> handlerWhenSuccess, params IResult[] results)
    {
        var hasAnyErrors = results.Any(x => !x.IsSuccess);

        if (!hasAnyErrors)
            return handlerWhenSuccess();

        var allErrors = results.SelectMany(x => x.Errors).ToArray();
        return Result<T>.Failure(allErrors);
    }
}