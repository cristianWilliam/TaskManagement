using FluentValidation.Results;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Core.ErrorManagement;

public static class ValidationResultExtensions
{
    public static Result<T> ToValidationErrorsResult<T>(
        this ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .Select(error => new ValidationError(error.ErrorMessage))
            .ToArray<IError>();

        return Result<T>.Failure(errors);
    }
}