namespace TaskManagement.Core.Tests.ErrorManagement.ResultPattern;

public class ResultTests
{
    private const string ErrorMessage = "Test error message";
    
    [Fact]
    public void Implicit_Should_CreateSuccessResult_When_ValueIsProvided()
    {
        // Arrange
        const string value = "Test value";
        
        // Act
        Result<string> result = value;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
        Assert.Equal(value, result.Value);
    }
    
    [Fact]
    public void Failure_Should_CreateFailureResult_When_SingleErrorIsProvided()
    {
        // Arrange
        var error = new DomainError(ErrorMessage);
        
        // Act
        var result = Result<string>.Failure(error);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal(ErrorMessage, result.Errors[0].Error);
        Assert.Equal(ErrorType.DomainError, result.Errors[0].Type);
        Assert.Null(result.Value);
    }
    
    [Fact]
    public void Failure_Should_CreateFailureResult_When_MultipleErrorsAreProvided()
    {
        // Arrange
        var errors = new IError[]
        {
            new DomainError("Error 1"),
            new ValidationError("Error 2")
        };
        
        // Act
        var result = Result<string>.Failure(errors);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal("Error 1", result.Errors[0].Error);
        Assert.Equal(ErrorType.DomainError, result.Errors[0].Type);
        Assert.Equal("Error 2", result.Errors[1].Error);
        Assert.Equal(ErrorType.ValidationError, result.Errors[1].Type);
        Assert.Null(result.Value);
    }
}
