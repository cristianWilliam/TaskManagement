namespace TaskManagement.Domain.Tests;

public class CardTests
{
    private readonly Faker _faker;
    private readonly Guid _cardId;
    private readonly CardDescription _validDescription;
    private readonly CardResponsible _validResponsible;
    private readonly DateTime _fixedDateTime;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CardTests()
    {
        _faker = new Faker();
        _cardId = _faker.Random.Guid();
        _validDescription = CardDescription.Create(_faker.Lorem.Sentence()).Value!;
        _validResponsible = CardResponsible.Create(_faker.Person.FullName).Value!;
        _fixedDateTime = _faker.Date.Recent();
        _dateTimeProvider = new MockDateTimeProvider(_fixedDateTime);
    }
    
    [Fact]
    public void CreateToDoCard_Should_ReturnSuccessResult_When_InputsAreValid()
    {
        // Act
        var result = Card.CreateToDoCard(_cardId, _validDescription, _validResponsible, _dateTimeProvider);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_cardId, result.Value!.Id);
        Assert.Equal(_validDescription, result.Value.Description);
        Assert.Equal(_validResponsible, result.Value.Responsible);
        Assert.Equal(CardStatus.Todo, result.Value.Status);
        Assert.Equal(_fixedDateTime, result.Value.CreatedOnUtc);
    }
    
    [Fact]
    public void CreateToDoCard_Should_AddCardCreatedEvent_When_CardIsCreated()
    {
        // Act
        var result = Card.CreateToDoCard(_cardId, _validDescription, _validResponsible, _dateTimeProvider);
        
        // Assert
        Assert.Single(result.Value!.DomainEvents);
        var domainEvent = result.Value.DomainEvents.First() as CardCreatedEvent;
        Assert.NotNull(domainEvent);
        Assert.Equal(_cardId, domainEvent!.CardId);
        Assert.Equal(_validDescription.Value, domainEvent.Description);
        Assert.Equal(_validResponsible.Value, domainEvent.Responsible);
        Assert.Equal(_fixedDateTime, domainEvent.CreationDateUtc);
        Assert.Equal(CardStatus.Todo, domainEvent.Status);
    }
    
    [Fact]
    public void UpdateStatus_Should_ReturnSuccessResult_When_CardIsNotDone()
    {
        // Arrange
        var card = Card.CreateToDoCard(_cardId, _validDescription, _validResponsible, _dateTimeProvider).Value!;
        
        // Act
        var result = card.UpdateStatus(CardStatus.InProgress, _dateTimeProvider);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(CardStatus.InProgress, card.Status);
        Assert.Equal(_fixedDateTime, card.UpdatedOnUtc);
    }
    
    [Fact]
    public void UpdateStatus_Should_AddCardStatusUpdatedEvent_When_StatusIsUpdated()
    {
        // Arrange
        var card = Card.CreateToDoCard(_cardId, _validDescription, _validResponsible, _dateTimeProvider).Value!;
        card.ClearEvents(); // Clear the creation event
        
        // Act
        var result = card.UpdateStatus(CardStatus.InProgress, _dateTimeProvider);
        
        // Assert
        Assert.Single(card.DomainEvents);
        var domainEvent = card.DomainEvents.First() as CardStatusUpdatedEvent;
        Assert.NotNull(domainEvent);
        Assert.Equal(_cardId, domainEvent!.CardId);
        Assert.Equal(CardStatus.Todo, domainEvent.OldStatus);
        Assert.Equal(CardStatus.InProgress, domainEvent.NewStatus);
    }
    
    [Fact]
    public void UpdateStatus_Should_ReturnFailureResult_When_CardIsDone()
    {
        // Arrange
        var card = Card.CreateToDoCard(_cardId, _validDescription, _validResponsible, _dateTimeProvider).Value!;
        card.UpdateStatus(CardStatus.Done, _dateTimeProvider); // First update to Done
        card.ClearEvents(); // Clear previous events
        
        // Act
        var result = card.UpdateStatus(CardStatus.InProgress, _dateTimeProvider);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.DomainError);
        Assert.Equal(CardStatus.Done, card.Status); // Status should not change
    }
    
    private class MockDateTimeProvider : IDateTimeProvider
    {
        public MockDateTimeProvider(DateTime fixedDateTime)
        {
            UtcNow = fixedDateTime;
        }

        public DateTime UtcNow { get; }
    }
}
