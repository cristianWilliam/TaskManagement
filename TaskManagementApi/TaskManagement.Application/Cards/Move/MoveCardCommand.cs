using MediatR;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Core.ErrorManagement.ResultPattern;
using TaskManagement.Domain;

namespace TaskManagement.Application.Cards.Move;

public record MoveCardCommand(Guid CardId, CardStatus NextStatus)
    : IRequest<Result<MoveCardDto>>;