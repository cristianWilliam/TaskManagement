namespace TaskManagement.Domain.Tests.ValueObjects;

public class CardResponsibleTests
{
    private readonly Faker _faker;
    private readonly string _validResponsible;
    private readonly string _emptyResponsible = string.Empty;
    
    public CardResponsibleTests()
    {
        _faker = new Faker();
        _validResponsible = _faker.Person.FullName;
    }
    
    [Fact]
    public void Create_Should_ReturnSuccessResult_When_ResponsibleIsValid()
    {
        // Act
        var result = CardResponsible.Create(_validResponsible);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_validResponsible, result.Value!.Value);
    }
    
    [Fact]
    public void Create_Should_ReturnFailureResult_When_ResponsibleIsEmpty()
    {
        // Act
        var result = CardResponsible.Create(_emptyResponsible);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.ValidationError);
    }
    
    [Fact]
    public void ToString_Should_ReturnResponsibleValue_When_ValueIsNotNull()
    {
        // Arrange
        var responsible = CardResponsible.Create(_validResponsible).Value!;
        
        // Act
        var result = responsible.ToString();
        
        // Assert
        Assert.Equal(_validResponsible, result);
    }
    
    [Fact]
    public void ImplicitOperator_Should_ConvertToString_When_Used()
    {
        // Arrange
        var responsible = CardResponsible.Create(_validResponsible).Value!;
        
        // Act
        string result = responsible;
        
        // Assert
        Assert.Equal(_validResponsible, result);
    }
}
