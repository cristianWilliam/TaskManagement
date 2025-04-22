namespace TaskManagement.Domain.Tests.Events;

public class DomainEventsTests
{
    private readonly Faker _faker;
    
    public DomainEventsTests()
    {
        _faker = new Faker();
    }
    
    [Fact]
    public void CardCreatedEvent_Should_HaveCorrectProperties_When_Created()
    {
        // Arrange
        var cardId = _faker.Random.Guid();
        var description = _faker.Lorem.Sentence();
        var responsible = _faker.Person.FullName;
        var creationDate = _faker.Date.Recent();
        var status = CardStatus.Todo;
        
        // Act
        var cardCreatedEvent = new CardCreatedEvent(cardId, description, responsible, creationDate, status);
        
        // Assert
        Assert.Equal(cardId, cardCreatedEvent.CardId);
        Assert.Equal(description, cardCreatedEvent.Description);
        Assert.Equal(responsible, cardCreatedEvent.Responsible);
        Assert.Equal(creationDate, cardCreatedEvent.CreationDateUtc);
        Assert.Equal(status, cardCreatedEvent.Status);
    }
    
    [Fact]
    public void CardStatusUpdatedEvent_Should_HaveCorrectProperties_When_Created()
    {
        // Arrange
        var cardId = _faker.Random.Guid();
        var oldStatus = CardStatus.Todo;
        var newStatus = CardStatus.InProgress;
        
        // Act
        var cardStatusUpdatedEvent = new CardStatusUpdatedEvent(cardId, oldStatus, newStatus);
        
        // Assert
        Assert.Equal(cardId, cardStatusUpdatedEvent.CardId);
        Assert.Equal(oldStatus, cardStatusUpdatedEvent.OldStatus);
        Assert.Equal(newStatus, cardStatusUpdatedEvent.NewStatus);
    }
    
    [Fact]
    public void BaseDomainEvents_Should_AddAndClearEvents_When_Used()
    {
        // Arrange
        var testEntity = new TestEntity();
        var domainEvent = new CardCreatedEvent(
            _faker.Random.Guid(),
            _faker.Lorem.Sentence(),
            _faker.Person.FullName,
            _faker.Date.Recent(),
            CardStatus.Todo);
        
        // Act - Add event
        testEntity.AddTestEvent(domainEvent);
        
        // Assert - Event was added
        Assert.Single(testEntity.DomainEvents);
        Assert.Same(domainEvent, testEntity.DomainEvents.First());
        
        // Act - Clear events
        testEntity.ClearEvents();
        
        // Assert - Events were cleared
        Assert.Empty(testEntity.DomainEvents);
    }
    
    // Test helper class that inherits from BaseDomainEvents
    private class TestEntity : BaseDomainEvents
    {
        public void AddTestEvent(IDomainEvent domainEvent)
        {
            AddDomainEvent(domainEvent);
        }
    }
}
