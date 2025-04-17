using TaskManagement.Domain;

namespace TaskManagement.Application.Cards;

public sealed record CardDto(
    Guid CardId,
    string Description,
    string Responsible,
    DateTime CreationDateUtc,
    CardStatus Status);