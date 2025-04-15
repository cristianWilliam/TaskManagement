using MediatR;

using Microsoft.EntityFrameworkCore;

using TaskManagement.Application.Tasks.Errors;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain;
using TaskManagement.Infra;

namespace TaskManagement.Application.Tasks.Move;

public record MoveCardCommand(Guid CardId, CardStatus NextStatus)
    : IRequest<Result<MoveCardDto>>;

internal sealed class
    MoveCardCommandHandler : IRequestHandler<MoveCardCommand,
    Result<MoveCardDto>>
{
    public readonly IDateTimeProvider _dateTimeProvider;
    public readonly AppDbContext _dbContext;

    public MoveCardCommandHandler(AppDbContext dbContext,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<MoveCardDto>> Handle(MoveCardCommand request,
        CancellationToken cancellationToken)
    {
        var card =
            await _dbContext.TaskCards.SingleOrDefaultAsync(
                card => card.Id == request.CardId, cancellationToken);
        if (card == null)
        {
            return Result<MoveCardDto>.Failure(
                new CardNotFoundError(request.CardId));
        }

        // Try Update
        if (request.NextStatus != card.Status)
        {
            var result =
                card.UpdateStatus(request.NextStatus, _dateTimeProvider);
            if (result.IsFailure)
            {
                return Result<MoveCardDto>.Failure(result.Errors);
            }
        }

        // Save
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new MoveCardDto(card.Id, card.Status);
    }
}