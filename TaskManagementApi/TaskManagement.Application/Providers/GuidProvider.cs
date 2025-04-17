namespace TaskManagement.Application.Providers;

internal sealed class GuidProvider : IGuidProvider
{
    public Guid GenerateSequential() => Guid.NewGuid(); // Can be sequential in a future, maybe based on timestamp.
}