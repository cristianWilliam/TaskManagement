namespace TaskManagement.Application.Tests.Providers;

public class GuidProviderTests
{
    [Fact]
    public void GenerateSequential_Should_ReturnNonEmptyGuid_When_Called()
    {
        // Arrange
        var provider = new GuidProvider();
        
        // Act
        var result = provider.GenerateSequential();
        
        // Assert
        Assert.NotEqual(Guid.Empty, result);
    }
    
    [Fact]
    public void GenerateSequential_Should_ReturnUniqueGuids_When_CalledMultipleTimes()
    {
        // Arrange
        var provider = new GuidProvider();
        var guids = new HashSet<Guid>();
        const int numberOfGuids = 100;
        
        // Act
        for (int i = 0; i < numberOfGuids; i++)
        {
            guids.Add(provider.GenerateSequential());
        }
        
        // Assert
        Assert.Equal(numberOfGuids, guids.Count);
    }
}
