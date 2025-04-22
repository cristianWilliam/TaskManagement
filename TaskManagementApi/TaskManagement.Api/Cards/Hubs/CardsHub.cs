using Microsoft.AspNetCore.SignalR;

namespace TaskManagement.Api.Cards.Hubs;

internal sealed class CardsHub : Hub
{
    public const string CardCreated = "CardCreated";
    public const string CardUpdated = "CardStatusUpdated";
}