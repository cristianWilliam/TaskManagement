using TaskManagement.Domain;

namespace TaskManagement.Application.Tasks.Move;

public record MoveCardDto(Guid CardId, CardStatus Status);