using FluentValidation.Results;
using TaskManagement.Api.Common;
using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Api.Extensions;

public static class ValidationResultExtensions
{
    public static CommonErrorResponse[] ToValidationErrorResponse(this ValidationResult validationResult)
    {
        return validationResult.ToValidationErrorsResult<CommonErrorResponse>().Errors
            .Select(x => new CommonErrorResponse(x.Type.ToString(), x.Error)).ToArray();
    }
}