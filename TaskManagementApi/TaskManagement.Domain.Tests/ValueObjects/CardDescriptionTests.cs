namespace TaskManagement.Domain.Tests.ValueObjects;

public class CardDescriptionTests
{
    private readonly Faker _faker;
    private readonly string _validDescription;
    private readonly string _emptyDescription = string.Empty;
    private readonly string _longDescription;
    
    public CardDescriptionTests()
    {
        _faker = new Faker();
        _validDescription = _faker.Lorem.Sentence();
        _longDescription = _faker.Lorem.Paragraphs(10); // Generates a very long text that exceeds 250 chars
    }
    
    [Fact]
    public void Create_Should_ReturnSuccessResult_When_DescriptionIsValid()
    {
        // Act
        var result = CardDescription.Create(_validDescription);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_validDescription, result.Value!.Value);
    }
    
    [Fact]
    public void Create_Should_ReturnFailureResult_When_DescriptionIsEmpty()
    {
        // Act
        var result = CardDescription.Create(_emptyDescription);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.ValidationError);
    }
    
    [Fact]
    public void Create_Should_ReturnFailureResult_When_DescriptionIsTooLong()
    {
        // Act
        var result = CardDescription.Create(_longDescription);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.ValidationError);
    }
    
    [Fact]
    public void ToString_Should_ReturnDescriptionValue_When_ValueIsNotNull()
    {
        // Arrange
        var description = CardDescription.Create(_validDescription).Value!;
        
        // Act
        var result = description.ToString();
        
        // Assert
        Assert.Equal(_validDescription, result);
    }
    
    [Fact]
    public void ImplicitOperator_Should_ConvertToString_When_Used()
    {
        // Arrange
        var description = CardDescription.Create(_validDescription).Value!;
        
        // Act
        string result = description;
        
        // Assert
        Assert.Equal(_validDescription, result);
    }
}
