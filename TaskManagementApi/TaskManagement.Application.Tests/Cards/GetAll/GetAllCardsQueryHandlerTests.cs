namespace TaskManagement.Application.Tests.Cards.GetAll;

public class GetAllCardsQueryHandlerTests : IClassFixture<DbContextFixture>
{
    private readonly DbContextFixture _fixture;
    
    public GetAllCardsQueryHandlerTests(DbContextFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Handle_Should_ReturnAllCards_When_CardsExist()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        var dateTimeProvider = _fixture.CreateDateTimeProvider();
        
        // Create cards in the database
        var cards = new List<Card>();
        for (int i = 0; i < 3; i++)
        {
            var cardId = _fixture.Faker.Random.Guid();
            var description = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
            var responsible = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
            var card = Card.CreateToDoCard(cardId, description, responsible, dateTimeProvider).Value!;
            cards.Add(card);
        }
        
        await dbContext.TaskCards.AddRangeAsync(cards);
        await dbContext.SaveChangesAsync();
        
        // Create the query
        var query = new GetAllCardsQuery();
        
        // Create the handler
        var handler = new GetAllCardsQueryHandler(dbContext);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(cards.Count, result.Length);
        
        foreach (var card in cards)
        {
            Assert.Contains(result, dto => 
                dto.CardId == card.Id && 
                dto.Description == card.Description.Value && 
                dto.Responsible == card.Responsible.Value && 
                dto.Status == card.Status);
        }
    }
    
    [Fact]
    public async Task Handle_Should_ReturnEmptyArray_When_NoCardsExist()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        
        // Create the query
        var query = new GetAllCardsQuery();
        
        // Create the handler
        var handler = new GetAllCardsQueryHandler(dbContext);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task Handle_Should_OrderCardsByUpdatedOrCreatedDate_When_CardsExist()
    {
        // Arrange
        var dbContext = _fixture.CreateDbContext();
        
        // Create cards with different dates
        var oldestDate = DateTime.UtcNow.AddDays(-3);
        var dateTimeProvider1 = _fixture.CreateDateTimeProvider(oldestDate);
        var cardId1 = _fixture.Faker.Random.Guid();
        var description1 = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible1 = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var card1 = Card.CreateToDoCard(cardId1, description1, responsible1, dateTimeProvider1).Value!;
        
        var middleDate = DateTime.UtcNow.AddDays(-2);
        var dateTimeProvider2 = _fixture.CreateDateTimeProvider(middleDate);
        var cardId2 = _fixture.Faker.Random.Guid();
        var description2 = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible2 = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var card2 = Card.CreateToDoCard(cardId2, description2, responsible2, dateTimeProvider2).Value!;
        
        var newestDate = DateTime.UtcNow.AddDays(-1);
        var dateTimeProvider3 = _fixture.CreateDateTimeProvider(newestDate);
        var cardId3 = _fixture.Faker.Random.Guid();
        var description3 = CardDescription.Create(_fixture.Faker.Lorem.Sentence()).Value!;
        var responsible3 = CardResponsible.Create(_fixture.Faker.Person.FullName).Value!;
        var card3 = Card.CreateToDoCard(cardId3, description3, responsible3, dateTimeProvider3).Value!;
        
        // Update card1 to be the most recent
        var updateDate = DateTime.UtcNow;
        var dateTimeProviderUpdate = _fixture.CreateDateTimeProvider(updateDate);
        card1.UpdateStatus(CardStatus.InProgress, dateTimeProviderUpdate);
        
        await dbContext.TaskCards.AddRangeAsync(new[] { card1, card2, card3 });
        await dbContext.SaveChangesAsync();
        
        // Create the query
        var query = new GetAllCardsQuery();
        
        // Create the handler
        var handler = new GetAllCardsQueryHandler(dbContext);
        
        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Length);
        
        // The first card should be card1 (most recently updated)
        Assert.Equal(cardId1, result[0].CardId);
        
        // The second card should be card3 (second most recent by creation date)
        Assert.Equal(cardId3, result[1].CardId);
        
        // The third card should be card2 (oldest by creation date)
        Assert.Equal(cardId2, result[2].CardId);
    }
}
