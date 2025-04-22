namespace TaskManagement.Core.Tests.ErrorManagement;

public class ValidationResultExtensionsTests
{
    private const string ErrorMessage1 = "Error message 1";
    private const string ErrorMessage2 = "Error message 2";
    
    [Fact]
    public void ToValidationErrorsResult_Should_ReturnFailureResult_When_ValidationHasErrors()
    {
        // Arrange
        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Property1", ErrorMessage1),
            new ValidationFailure("Property2", ErrorMessage2)
        };
        
        var validationResult = new ValidationResult(validationFailures);
        
        // Act
        var result = validationResult.ToValidationErrorsResult<string>();
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal(ErrorMessage1, result.Errors[0].Error);
        Assert.Equal(ErrorType.ValidationError, result.Errors[0].Type);
        Assert.Equal(ErrorMessage2, result.Errors[1].Error);
        Assert.Equal(ErrorType.ValidationError, result.Errors[1].Type);
    }
    
    [Fact]
    public void ToValidationErrorsResult_Should_ReturnEmptyErrorsList_When_ValidationHasNoErrors()
    {
        // Arrange
        var validationResult = new ValidationResult();
        
        // Act
        var result = validationResult.ToValidationErrorsResult<string>();
        
        // Assert
        // When there are no validation errors, the result still has an empty errors list
        Assert.Empty(result.Errors);
    }
}
