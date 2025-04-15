using FluentValidation.Results;

namespace TaskManagement.Core.ErrorManagement;

public static class ValidationResultExtensions
{
    public static Result<T> ToValidationErrorsResult<T>(
        this ValidationResult validationResult)
    {
        IError[] errors = validationResult.Errors
            .Select(error => new ValidationError(error.ErrorMessage))
            .ToArray<IError>();

        return Result<T>.Failure(errors);
    }
}