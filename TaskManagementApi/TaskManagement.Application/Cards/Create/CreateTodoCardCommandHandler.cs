using MediatR;
using TaskManagement.Application.Providers;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain;
using TaskManagement.Infra;

namespace TaskManagement.Application.Cards.Create;

internal sealed class CreateTodoCardCommandHandler : IRequestHandler<
    CreateTodoCardCommand, Result<CardDto>>
{
    private readonly AppDbContext _context;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IGuidProvider _guidProvider;

    public CreateTodoCardCommandHandler(IDateTimeProvider dateTimeProvider,
        AppDbContext context, IGuidProvider guidProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _context = context;
        _guidProvider = guidProvider;
    }

    public async Task<Result<CardDto>> Handle(
        CreateTodoCardCommand request, CancellationToken cancellationToken)
    {
        var newCardId = _guidProvider.GenerateSequential();

        var newCardResult = Card.CreateToDoCard(newCardId, request.Description,
            request.Responsible, _dateTimeProvider);

        if (newCardResult.IsFailure)
            return Result<CardDto>.Failure(newCardResult.Errors);

        var newCard = newCardResult.Value!;

        await _context.TaskCards.AddAsync(newCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CardDto(newCard.Id, newCard.Description.ToString()!,
            newCard.Responsible.ToString()!, newCard.CreatedOnUtc,
            newCard.Status);
    }
}