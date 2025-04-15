using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.Validators;

internal interface ISpecification<T>
{
    Result<bool> IsSatisfiedBy(T card);
}