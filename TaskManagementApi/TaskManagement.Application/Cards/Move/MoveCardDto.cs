using TaskManagement.Domain;

namespace TaskManagement.Application.Cards.Move;

public record MoveCardDto(Guid CardId, CardStatus NewStatus);