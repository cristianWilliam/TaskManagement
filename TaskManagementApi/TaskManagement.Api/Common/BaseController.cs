using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Api.Common;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult GetResponseError(IError[] errors)
    {
        var errorsResponse = errors.Select(x => new CommonErrorResponse(x.Type.ToString(), x.Error))
            .ToArray();

        if (errors.Any(x => x.Type == ErrorType.DomainError))
            return Conflict(errorsResponse);

        if (errors.Any(x => x.Type == ErrorType.NotFoundError))
            return NotFound(errorsResponse);

        return BadRequest(errorsResponse);
    }

    protected IActionResult GetResponseError(List<IError> errors)
    {
        return GetResponseError(errors.ToArray());
    }
}