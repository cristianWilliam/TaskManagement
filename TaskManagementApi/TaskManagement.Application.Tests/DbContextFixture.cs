using Microsoft.EntityFrameworkCore;
using TaskManagement.Persistence;

namespace TaskManagement.Application.Tests;

public class DbContextFixture : IDisposable
{
    private readonly string _databaseName;
    private readonly DbContextOptions<AppDbContext> _options;
    public readonly Faker Faker;
    
    public DbContextFixture()
    {
        Faker = new Faker();
        _databaseName = Guid.NewGuid().ToString();
        
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(_databaseName)
            .Options;
    }
    
    public AppDbContext CreateDbContext()
    {
        // Cada chamada cria uma nova instância do DbContext com um banco de dados único
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        
        return new AppDbContext(options);
    }
    
    public IDateTimeProvider CreateDateTimeProvider(DateTime? fixedDateTime = null)
    {
        var dateTime = fixedDateTime ?? Faker.Date.Recent();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(dateTime);
        return dateTimeProvider;
    }
    
    public IGuidProvider CreateGuidProvider(Guid? fixedGuid = null)
    {
        var guidProvider = Substitute.For<IGuidProvider>();
        guidProvider.GenerateSequential().Returns(fixedGuid ?? Faker.Random.Guid());
        return guidProvider;
    }
    
    public ICardsNotificationService CreateCardsNotificationService()
    {
        return Substitute.For<ICardsNotificationService>();
    }
    
    public void Dispose()
    {
        // Cleanup if needed
    }
}
