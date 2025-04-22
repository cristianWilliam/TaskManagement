namespace TaskManagement.Application.Tests.Providers;

public class DateTimeProviderTests
{
    [Fact]
    public void UtcNow_Should_ReturnCurrentUtcTime_When_Accessed()
    {
        // Arrange
        var beforeTest = DateTime.UtcNow.AddSeconds(-1);
        var provider = new DateTimeProvider();
        var afterTest = DateTime.UtcNow.AddSeconds(1);
        
        // Act
        var result = provider.UtcNow;
        
        // Assert
        Assert.True(result >= beforeTest);
        Assert.True(result <= afterTest);
    }
    
    [Fact]
    public void UtcNow_Should_ReturnUtcKind_When_Accessed()
    {
        // Arrange
        var provider = new DateTimeProvider();
        
        // Act
        var result = provider.UtcNow;
        
        // Assert
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }
}
