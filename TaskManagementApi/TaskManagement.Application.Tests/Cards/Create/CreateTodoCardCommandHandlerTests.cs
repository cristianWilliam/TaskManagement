namespace TaskManagement.Application.Tests.Cards.Create;

public class CreateTodoCardCommandHandlerTests : IClassFixture<DbContextFixture>
{
    private readonly DbContextFixture _fixture;
    
    public CreateTodoCardCommandHandlerTests(DbContextFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_CommandIsValid()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var dateTimeProvider = _fixture.CreateDateTimeProvider();
        var guidProvider = _fixture.CreateGuidProvider();
        var cardId = _fixture.Faker.Random.Guid();
        guidProvider.GenerateSequential().Returns(cardId);
        
        var description = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var command = new CreateTodoCardCommand(description, responsible);
        
        var handler = new CreateTodoCardCommandHandler(dateTimeProvider, dbContext, guidProvider);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(cardId, result.Value!.CardId);
        Assert.Equal(description.Value, result.Value.Description);
        Assert.Equal(responsible.Value, result.Value.Responsible);
        Assert.Equal(CardStatus.Todo, result.Value.Status);
        
        // Verify card was saved to database
        var savedCard = await dbContext.TaskCards.FindAsync(cardId);
        Assert.NotNull(savedCard);
        Assert.Equal(cardId, savedCard!.Id);
        Assert.Equal(description, savedCard.Description);
        Assert.Equal(responsible, savedCard.Responsible);
        Assert.Equal(CardStatus.Todo, savedCard.Status);
    }
}
