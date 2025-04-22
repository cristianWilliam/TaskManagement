using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Api.Cards.Controllers;
using TaskManagement.Application.Cards;
using TaskManagement.Application.Cards.Move;
using TaskManagement.Domain;
using TaskManagement.Domain.ValueObjects;
using TaskManagement.Persistence;

namespace TaskManagementApi.Tests.Integration;

[Collection("AppDb")]
public class CardsControllerTests
{
    private readonly AppDbContext _appDb;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public CardsControllerTests(AppFactory fixture)
    {
        var scope = fixture.Services.CreateScope();
        _appDb = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        _client = fixture.CreateClient();

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    [Fact]
    public async Task Given_ValidRequest_When_GetCards_Then_ReturnsOkWithCards()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var description = CardDescription.Create("Test Description").Value;
        var responsible = CardResponsible.Create("Test Responsible").Value;
        var cardResult = Card.CreateToDoCard(cardId, description!, responsible!, _dateTimeProvider).Value!;

        await _appDb.TaskCards.AddAsync(cardResult);
        await _appDb.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/Cards");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var cards = await response.Content.ReadFromJsonAsync<CardDto[]>(_jsonOptions);
        
        Assert.NotNull(cards);
        Assert.Contains(cards, c => c is { Description: "Test Description", Responsible: "Test Responsible" });
    }

    [Fact]
    public async Task Given_ValidRequest_When_CreateTodoCard_Then_ReturnsCreatedWithCard()
    {
        // Arrange
        var request = new CreateTodoCardRequest("New Card", "John Doe");

        // Act
        var response = await _client.PostAsJsonAsync("/api/Cards", request, _jsonOptions);
        
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var card = await response.Content.ReadFromJsonAsync<CardDto>(_jsonOptions);
        
        Assert.NotNull(card);
        Assert.Equal("New Card", card.Description);
        Assert.Equal("John Doe", card.Responsible);
        Assert.Equal(CardStatus.Todo, card.Status);

        // Assert from DB
        _appDb.ChangeTracker.Clear();
        var createdCard = await _appDb.TaskCards.FirstOrDefaultAsync(x => x.Id == card.CardId);
        Assert.NotNull(createdCard);
        Assert.Equal("New Card", createdCard.Description);
        Assert.Equal("John Doe", createdCard.Responsible);
    }

    [Fact]
    public async Task Given_ValidRequest_When_MoveCard_Then_ReturnsOkWithUpdatedCard()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var description = CardDescription.Create("Move Card Test").Value!;
        var responsible = CardResponsible.Create("Test User").Value!;
        
        var cardResult = Card.CreateToDoCard(cardId, description, responsible, _dateTimeProvider).Value!;
        
        await _appDb.TaskCards.AddAsync(cardResult);
        await _appDb.SaveChangesAsync();

        var request = new MoveCardRequest(cardResult.Id, CardStatus.InProgress);

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/Cards/{cardResult.Id}", request, _jsonOptions);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var updatedCard = await response.Content.ReadFromJsonAsync<MoveCardDto>(_jsonOptions);
        
        Assert.NotNull(updatedCard);
        Assert.Equal(cardResult.Id, updatedCard.CardId);
        Assert.Equal(CardStatus.InProgress, updatedCard.NewStatus);

        // Assert from DB
        _appDb.ChangeTracker.Clear();
        var cardFromDb = await _appDb.TaskCards.FirstOrDefaultAsync(x => x.Id == updatedCard.CardId);
        Assert.NotNull(cardFromDb);
        Assert.Equal(CardStatus.InProgress, cardFromDb.Status);
    }
}