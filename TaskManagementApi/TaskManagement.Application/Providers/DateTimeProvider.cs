using TaskManagement.Domain;

namespace TaskManagement.Application.Providers;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}