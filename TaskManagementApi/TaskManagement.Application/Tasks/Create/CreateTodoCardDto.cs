using TaskManagement.Domain;

namespace TaskManagement.Application.Tasks.Create;

public sealed record CreateTodoCardDto(
    Guid Id,
    string Description,
    string Responsible,
    DateTime CreationDateUtc,
    CardStatus Status);