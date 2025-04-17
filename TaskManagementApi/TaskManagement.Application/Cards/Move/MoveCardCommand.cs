using MediatR;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain;

namespace TaskManagement.Application.Cards.Move;

public record MoveCardCommand(Guid CardId, CardStatus NextStatus)
    : IRequest<Result<MoveCardDto>>;