namespace TaskManagement.Application.Providers;

public interface IGuidProvider
{
    Guid GenerateSequential();
}