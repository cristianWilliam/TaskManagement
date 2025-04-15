namespace TaskManagement.Domain;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}