namespace TaskManagement.Application.Providers;

internal sealed class GuidProvider : IGuidProvider
{
    public Guid GenerateSequential()
    {
        return Guid.NewGuid();
        // Can be sequential in a future, maybe based on timestamp.
    }
}