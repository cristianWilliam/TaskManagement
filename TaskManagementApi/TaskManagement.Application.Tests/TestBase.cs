using Microsoft.EntityFrameworkCore;
using TaskManagement.Persistence;

namespace TaskManagement.Application.Tests;

public abstract class TestBase
{
    protected readonly Faker _faker;
    
    protected TestBase()
    {
        _faker = new Faker();
    }
    
    protected AppDbContext CreateDbContext(string databaseName = null)
    {
        databaseName ??= Guid.NewGuid().ToString();
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        
        return new AppDbContext(options);
    }
    
    protected IDateTimeProvider CreateDateTimeProvider(DateTime? fixedDateTime = null)
    {
        var dateTime = fixedDateTime ?? _faker.Date.Recent();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(dateTime);
        return dateTimeProvider;
    }
    
    protected IGuidProvider CreateGuidProvider(Guid? fixedGuid = null)
    {
        var guidProvider = Substitute.For<IGuidProvider>();
        guidProvider.GenerateSequential().Returns(fixedGuid ?? _faker.Random.Guid());
        return guidProvider;
    }
    
    protected ICardsNotificationService CreateCardsNotificationService()
    {
        return Substitute.For<ICardsNotificationService>();
    }
}
