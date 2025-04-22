namespace TaskManagement.Core.Tests.ErrorManagement.ResultPattern;

public class ResultExtensionsTests
{
    private const string SuccessValue = "Success value";
    private const string ErrorMessage = "Error message";
    
    [Fact]
    public void Match_Should_CallSuccessHandler_When_ResultIsSuccess()
    {
        // Arrange
        Result<string> result = SuccessValue;
        var successHandlerCalled = false;
        var errorHandlerCalled = false;
        
        // Act
        var matchResult = result.Match(
            onErrorHandler: _ => 
            {
                errorHandlerCalled = true;
                return "Error";
            },
            onSuccessHandler: value => 
            {
                successHandlerCalled = true;
                return value;
            });
        
        // Assert
        Assert.True(successHandlerCalled);
        Assert.False(errorHandlerCalled);
        Assert.Equal(SuccessValue, matchResult);
    }
    
    [Fact]
    public void Match_Should_CallErrorHandler_When_ResultIsFailure()
    {
        // Arrange
        var error = new DomainError(ErrorMessage);
        var result = Result<string>.Failure(error);
        var successHandlerCalled = false;
        var errorHandlerCalled = false;
        
        // Act
        var matchResult = result.Match(
            onErrorHandler: errors => 
            {
                errorHandlerCalled = true;
                return errors[0].Error;
            },
            onSuccessHandler: value => 
            {
                successHandlerCalled = true;
                return value;
            });
        
        // Assert
        Assert.False(successHandlerCalled);
        Assert.True(errorHandlerCalled);
        Assert.Equal(ErrorMessage, matchResult);
    }
}
