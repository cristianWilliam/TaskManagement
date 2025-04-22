namespace TaskManagement.Core.Tests.ErrorManagement;

public class ResultHelpersTests
{
    private const string SuccessValue = "Success value";
    private const string ErrorMessage1 = "Error message 1";
    private const string ErrorMessage2 = "Error message 2";
    
    [Fact]
    public void MergeResults_Should_ReturnSuccessResult_When_AllResultsAreSuccess()
    {
        // Arrange
        Result<int> result1 = 1;
        Result<string> result2 = "test";
        
        // Act
        var mergedResult = ResultHelpers.MergeResults(
            handlerWhenSuccess: () => SuccessValue,
            result1, result2);
        
        // Assert
        Assert.True(mergedResult.IsSuccess);
        Assert.Equal(SuccessValue, mergedResult.Value);
    }
    
    [Fact]
    public void MergeResults_Should_ReturnFailureWithAllErrors_When_AnyResultIsFailure()
    {
        // Arrange
        Result<int> result1 = 1;
        var error = new DomainError(ErrorMessage1);
        var result2 = Result<string>.Failure(error);
        
        // Act
        var mergedResult = ResultHelpers.MergeResults(
            handlerWhenSuccess: () => SuccessValue,
            result1, result2);
        
        // Assert
        Assert.True(mergedResult.IsFailure);
        Assert.Single(mergedResult.Errors);
        Assert.Equal(ErrorMessage1, mergedResult.Errors[0].Error);
    }
    
    [Fact]
    public void MergeResults_Should_CombineAllErrors_When_MultipleResultsHaveErrors()
    {
        // Arrange
        var error1 = new DomainError(ErrorMessage1);
        var result1 = Result<int>.Failure(error1);
        
        var error2 = new ValidationError(ErrorMessage2);
        var result2 = Result<string>.Failure(error2);
        
        // Act
        var mergedResult = ResultHelpers.MergeResults(
            handlerWhenSuccess: () => SuccessValue,
            result1, result2);
        
        // Assert
        Assert.True(mergedResult.IsFailure);
        Assert.Equal(2, mergedResult.Errors.Count);
        Assert.Equal(ErrorMessage1, mergedResult.Errors[0].Error);
        Assert.Equal(ErrorType.DomainError, mergedResult.Errors[0].Type);
        Assert.Equal(ErrorMessage2, mergedResult.Errors[1].Error);
        Assert.Equal(ErrorType.ValidationError, mergedResult.Errors[1].Type);
    }
}
