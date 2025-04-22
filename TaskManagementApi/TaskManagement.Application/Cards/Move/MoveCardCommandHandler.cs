using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Cards.Errors;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain;
using TaskManagement.Persistence;

namespace TaskManagement.Application.Cards.Move;

internal sealed class MoveCardCommandHandler : IRequestHandler<MoveCardCommand, Result<MoveCardDto>>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AppDbContext _dbContext;

    public MoveCardCommandHandler(AppDbContext dbContext,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<MoveCardDto>> Handle(MoveCardCommand request,
        CancellationToken cancellationToken)
    {
        var card = await _dbContext.TaskCards.SingleOrDefaultAsync(card => card.Id == request.CardId, cancellationToken);
        if (card is null) return Result<MoveCardDto>.Failure(new CardNotFoundError(request.CardId));

        if (request.NextStatus == card.Status)
            return new MoveCardDto(card.Id, card.Status);

        // Try update the Status
        var cardUpdateStatusResult = card.UpdateStatus(request.NextStatus, _dateTimeProvider);

        if (cardUpdateStatusResult.IsFailure)
            return Result<MoveCardDto>.Failure(cardUpdateStatusResult.Errors);

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Save
        return new MoveCardDto(card.Id, card.Status);
    }
}