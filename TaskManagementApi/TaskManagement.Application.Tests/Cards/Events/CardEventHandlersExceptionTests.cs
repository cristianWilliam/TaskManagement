namespace TaskManagement.Application.Tests.Cards.Events;

public class CardEventHandlersExceptionTests
{
    [Fact]
    public async Task CardCreatedEventHandler_Should_PropagateException_When_NotificationServiceFails()
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
        
        // Configure service to throw an exception
        var expectedException = new InvalidOperationException("Notification service failed");
        notificationService
            .NotifyCardCreated(Arg.Any<CardCreatedEvent>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(cardCreatedEvent, CancellationToken.None));
        
        Assert.Same(expectedException, exception);
    }
    
    [Fact]
    public async Task CardStatusUpdatedEventHandler_Should_PropagateException_When_NotificationServiceFails()
    {
        // Arrange
        var notificationService = Substitute.For<ICardsNotificationService>();
        var handler = new CardStatusUpdatedEventHandler(notificationService);
        
        var cardId = Guid.NewGuid();
        var oldStatus = CardStatus.Todo;
        var newStatus = CardStatus.InProgress;
        
        var cardStatusUpdatedEvent = new CardStatusUpdatedEvent(cardId, oldStatus, newStatus);
        
        // Configure service to throw an exception
        var expectedException = new InvalidOperationException("Notification service failed");
        notificationService
            .NotifyCardStatusUpdated(Arg.Any<CardStatusUpdatedEvent>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(expectedException));
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            handler.Handle(cardStatusUpdatedEvent, CancellationToken.None));
        
        Assert.Same(expectedException, exception);
    }
}
