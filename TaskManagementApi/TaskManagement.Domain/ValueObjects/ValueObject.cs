using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Domain.ValueObjects;

public interface IValueObject<T>
{
    string? Value { get; init; }
    static abstract Result<T> Create(string value);
}