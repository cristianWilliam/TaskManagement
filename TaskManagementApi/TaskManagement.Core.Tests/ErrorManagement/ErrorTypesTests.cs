namespace TaskManagement.Core.Tests.ErrorManagement;

public class ErrorTypesTests
{
    private const string ErrorMessage = "Test error message";
    
    [Fact]
    public void DomainError_Should_HaveCorrectErrorTypeAndMessage_When_Created()
    {
        // Arrange & Act
        var error = new DomainError(ErrorMessage);
        
        // Assert
        Assert.Equal(ErrorMessage, error.Error);
        Assert.Equal(ErrorType.DomainError, error.Type);
    }
    
    [Fact]
    public void ValidationError_Should_HaveCorrectErrorTypeAndMessage_When_Created()
    {
        // Arrange & Act
        var error = new ValidationError(ErrorMessage);
        
        // Assert
        Assert.Equal(ErrorMessage, error.Error);
        Assert.Equal(ErrorType.ValidationError, error.Type);
    }
    
    [Fact]
    public void DomainError_Should_BeEqualToAnotherDomainError_When_MessagesAreEqual()
    {
        // Arrange
        var error1 = new DomainError(ErrorMessage);
        var error2 = new DomainError(ErrorMessage);
        
        // Act & Assert
        Assert.Equal(error1, error2);
    }
    
    [Fact]
    public void ValidationError_Should_BeEqualToAnotherValidationError_When_MessagesAreEqual()
    {
        // Arrange
        var error1 = new ValidationError(ErrorMessage);
        var error2 = new ValidationError(ErrorMessage);
        
        // Act & Assert
        Assert.Equal(error1, error2);
    }
    
    [Fact]
    public void DomainError_Should_NotBeEqualToAnotherDomainError_When_MessagesAreDifferent()
    {
        // Arrange
        var error1 = new DomainError(ErrorMessage);
        var error2 = new DomainError("Different message");
        
        // Act & Assert
        Assert.NotEqual(error1, error2);
    }
    
    [Fact]
    public void ValidationError_Should_NotBeEqualToAnotherValidationError_When_MessagesAreDifferent()
    {
        // Arrange
        var error1 = new ValidationError(ErrorMessage);
        var error2 = new ValidationError("Different message");
        
        // Act & Assert
        Assert.NotEqual(error1, error2);
    }
}
