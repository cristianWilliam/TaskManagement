namespace TaskManagement.Application.Tests.Cards.Create;

public class CreateTodoCardCommandFixture : IDisposable
{
    public readonly Faker Faker;
    
    public CreateTodoCardCommandFixture()
    {
        Faker = new Faker();
    }
    
    public string GenerateValidDescription() => Faker.Lorem.Sentence();
    
    public string GenerateValidResponsible() => Faker.Person.FullName;
    
    public string GenerateInvalidDescription() => string.Empty;
    
    public string GenerateInvalidResponsible() => string.Empty;
    
    public void Dispose()
    {
        // Cleanup if needed
    }
}
