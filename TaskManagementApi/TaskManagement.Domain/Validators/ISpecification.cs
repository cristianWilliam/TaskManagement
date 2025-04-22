using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Domain.Validators;

internal interface ISpecification<T>
{
    Result<bool> IsSatisfiedBy(T card);
}