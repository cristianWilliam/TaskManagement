namespace TaskManagement.Domain.Tests.Validators;

public class CanUpdateStatusSpecificationTests
{
    private readonly Faker _faker;
    private readonly CanUpdateStatusSpecification _specification;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CanUpdateStatusSpecificationTests()
    {
        _faker = new Faker();
        _specification = new CanUpdateStatusSpecification();
        _dateTimeProvider = new MockDateTimeProvider(_faker.Date.Recent());
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnSuccess_When_CardStatusIsTodo()
    {
        // Arrange
        var description = CardDescription.Create(_faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_faker.Person.FullName).Value!;
        var card = Card.CreateToDoCard(_faker.Random.Guid(), description, responsible, _dateTimeProvider).Value!;
        
        // Act
        var result = _specification.IsSatisfiedBy(card);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnSuccess_When_CardStatusIsInProgress()
    {
        // Arrange
        var description = CardDescription.Create(_faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_faker.Person.FullName).Value!;
        var card = Card.CreateToDoCard(_faker.Random.Guid(), description, responsible, _dateTimeProvider).Value!;
        card.UpdateStatus(CardStatus.InProgress, _dateTimeProvider);
        
        // Act
        var result = _specification.IsSatisfiedBy(card);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value);
    }
    
    [Fact]
    public void IsSatisfiedBy_Should_ReturnFailure_When_CardStatusIsDone()
    {
        // Arrange
        var description = CardDescription.Create(_faker.Lorem.Sentence()).Value!;
        var responsible = CardResponsible.Create(_faker.Person.FullName).Value!;
        var card = Card.CreateToDoCard(_faker.Random.Guid(), description, responsible, _dateTimeProvider).Value!;
        card.UpdateStatus(CardStatus.Done, _dateTimeProvider);
        
        // Act
        var result = _specification.IsSatisfiedBy(card);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(result.Errors, e => e.Type == ErrorType.DomainError);
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
