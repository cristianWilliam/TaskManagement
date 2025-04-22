namespace TaskManagement.Application.Tests.Cards.Events;

public class CardEventHandlersTests
{
    [Fact]
    public async Task CardCreatedEventHandler_Should_NotifyCardCreated_When_EventIsReceived()
    {
        // Arrange
        var notificationService = Substitute.For<ICardsNotificationService>();
        var handler = new CardCreatedEventHandler(notificationService);
        
        var cardId = Guid.NewGuid();
        var description = "Test Description";
        var responsible = "Test User";
        var creationDate = DateTime.UtcNow;
        var status = CardStatus.Todo;
        
        var cardCreatedEvent = new CardCreatedEvent(cardId, description, responsible, creationDate, status);
        
        // Act
        await handler.Handle(cardCreatedEvent, CancellationToken.None);
        
        // Assert
        await notificationService.Received(1).NotifyCardCreated(
            Arg.Is<CardCreatedEvent>(e => 
                e.CardId == cardId && 
                e.Description == description && 
                e.Responsible == responsible && 
                e.CreationDateUtc == creationDate && 
                e.Status == status),
            Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CardStatusUpdatedEventHandler_Should_NotifyCardStatusUpdated_When_EventIsReceived()
    {
        // Arrange
        var notificationService = Substitute.For<ICardsNotificationService>();
        var handler = new CardStatusUpdatedEventHandler(notificationService);
        
        var cardId = Guid.NewGuid();
        var oldStatus = CardStatus.Todo;
        var newStatus = CardStatus.InProgress;
        
        var cardStatusUpdatedEvent = new CardStatusUpdatedEvent(cardId, oldStatus, newStatus);
        
        // Act
        await handler.Handle(cardStatusUpdatedEvent, CancellationToken.None);
        
        // Assert
        await notificationService.Received(1).NotifyCardStatusUpdated(
            Arg.Is<CardStatusUpdatedEvent>(e => 
                e.CardId == cardId && 
                e.OldStatus == oldStatus && 
                e.NewStatus == newStatus),
            Arg.Any<CancellationToken>());
    }
}
