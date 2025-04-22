namespace TaskManagement.Application.Tests.Cards.Create;

public class CreateTodoCardCommandTests : IClassFixture<CreateTodoCardCommandFixture>
{
    private readonly CreateTodoCardCommandFixture _fixture;
    
    public CreateTodoCardCommandTests(CreateTodoCardCommandFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public void TryCreate_Should_ReturnSuccessResult_When_InputsAreValid()
    {
        // Arrange
        var description = _fixture.GenerateValidDescription();
        var responsible = _fixture.GenerateValidResponsible();
        
        // Act
        var result = CreateTodoCardCommand.TryCreate(description, responsible);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(description, result.Value!.Description.Value);
        Assert.Equal(responsible, result.Value.Responsible.Value);
    }
    
    [Fact]
    public void TryCreate_Should_ReturnFailureResult_When_DescriptionIsInvalid()
    {
        // Arrange
        var description = _fixture.GenerateInvalidDescription();
        var responsible = _fixture.GenerateValidResponsible();
        
        // Act
        var result = CreateTodoCardCommand.TryCreate(description, responsible);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.ValidationError);
    }
    
    [Fact]
    public void TryCreate_Should_ReturnFailureResult_When_ResponsibleIsInvalid()
    {
        // Arrange
        var description = _fixture.GenerateValidDescription();
        var responsible = _fixture.GenerateInvalidResponsible();
        
        // Act
        var result = CreateTodoCardCommand.TryCreate(description, responsible);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.ValidationError);
    }
    
    [Fact]
    public void TryCreate_Should_CombineErrors_When_BothInputsAreInvalid()
    {
        // Arrange
        var description = _fixture.GenerateInvalidDescription();
        var responsible = _fixture.GenerateInvalidResponsible();
        
        // Act
        var result = CreateTodoCardCommand.TryCreate(description, responsible);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(2, result.Errors.Count);
        Assert.All(result.Errors, e => Assert.Equal(ErrorType.ValidationError, e.Type));
    }
}
