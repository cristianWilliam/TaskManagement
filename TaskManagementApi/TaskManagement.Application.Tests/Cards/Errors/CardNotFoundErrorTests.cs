namespace TaskManagement.Application.Tests.Cards.Errors;

public class CardNotFoundErrorTests
{
    [Fact]
    public void Constructor_Should_SetProperties_When_Called()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        
        // Act
        var error = new CardNotFoundError(cardId);
        
        // Assert
        Assert.Equal($"Card [{cardId}] not found", error.Error);
        Assert.Equal(ErrorType.NotFoundError, error.Type);
    }
    
    [Fact]
    public void Error_Should_ContainCardId_When_Created()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        
        // Act
        var error = new CardNotFoundError(cardId);
        
        // Assert
        Assert.Contains(cardId.ToString(), error.Error);
    }
}
