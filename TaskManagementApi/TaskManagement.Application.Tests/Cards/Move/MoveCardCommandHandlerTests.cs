namespace TaskManagement.Application.Tests.Cards.Move;

public class MoveCardCommandHandlerTests : IClassFixture<DbContextFixture>
{
    private readonly DbContextFixture _fixture;
    
    public MoveCardCommandHandlerTests(DbContextFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_When_CardExists()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var dateTimeProvider = _fixture.CreateDateTimeProvider();
        var cardId = _fixture.Faker.Random.Guid();
        
        // Create a card in the database
        var description = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var card = Card.CreateToDoCard(cardId, description, responsible, dateTimeProvider).Value!;
        
        await dbContext.TaskCards.AddAsync(card);
        await dbContext.SaveChangesAsync();
        
        // Create the command
        var command = new MoveCardCommand(cardId, CardStatus.InProgress);
        
        // Create the handler
        var handler = new MoveCardCommandHandler(dbContext, dateTimeProvider);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(cardId, result.Value!.CardId);
        Assert.Equal(CardStatus.InProgress, result.Value.NewStatus);
        
        // Verify card was updated in the database
        var updatedCard = await dbContext.TaskCards.FindAsync(cardId);
        Assert.NotNull(updatedCard);
        Assert.Equal(CardStatus.InProgress, updatedCard!.Status);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureResult_When_CardDoesNotExist()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var dateTimeProvider = _fixture.CreateDateTimeProvider();
        var nonExistentCardId = _fixture.Faker.Random.Guid();
        
        // Create the command
        var command = new MoveCardCommand(nonExistentCardId, CardStatus.InProgress);
        
        // Create the handler
        var handler = new MoveCardCommandHandler(dbContext, dateTimeProvider);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.NotFoundError);
        Assert.Contains(result.Errors, e => e is CardNotFoundError);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnSameStatus_When_NextStatusIsSameAsCurrent()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var dateTimeProvider = _fixture.CreateDateTimeProvider();
        var cardId = _fixture.Faker.Random.Guid();
        
        // Create a card in the database
        var description = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var card = Card.CreateToDoCard(cardId, description, responsible, dateTimeProvider).Value!;
        
        await dbContext.TaskCards.AddAsync(card);
        await dbContext.SaveChangesAsync();
        
        // Create the command with the same status
        var command = new MoveCardCommand(cardId, CardStatus.Todo);
        
        // Create the handler
        var handler = new MoveCardCommandHandler(dbContext, dateTimeProvider);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(cardId, result.Value!.CardId);
        Assert.Equal(CardStatus.Todo, result.Value.NewStatus);
        
        // Verify card status was not changed in the database
        var updatedCard = await dbContext.TaskCards.FindAsync(cardId);
        Assert.NotNull(updatedCard);
        Assert.Equal(CardStatus.Todo, updatedCard!.Status);
    }
    
    [Fact]
    public async Task Handle_Should_ReturnFailureResult_When_CardIsDone()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var dateTimeProvider = _fixture.CreateDateTimeProvider();
        var cardId = _fixture.Faker.Random.Guid();
        
        // Create a card in the database
        var description = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var card = Card.CreateToDoCard(cardId, description, responsible, dateTimeProvider).Value!;
        
        // Move the card to Done status
        card.UpdateStatus(CardStatus.Done, dateTimeProvider);
        
        await dbContext.TaskCards.AddAsync(card);
        await dbContext.SaveChangesAsync();
        
        // Create the command to move from Done to InProgress
        var command = new MoveCardCommand(cardId, CardStatus.InProgress);
        
        // Create the handler
        var handler = new MoveCardCommandHandler(dbContext, dateTimeProvider);
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.DomainError);
        
        // Verify card status was not changed in the database
        var updatedCard = await dbContext.TaskCards.FindAsync(cardId);
        Assert.NotNull(updatedCard);
        Assert.Equal(CardStatus.Done, updatedCard!.Status);
    }
}
