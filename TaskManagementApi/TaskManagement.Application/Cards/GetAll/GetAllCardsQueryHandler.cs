using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Infra;

namespace TaskManagement.Application.Cards.GetAll;

internal class GetAllCardsQueryHandler : IRequestHandler<GetAllCardsQuery, CardDto[]>
{
    private readonly AppDbContext _appDb;

    public GetAllCardsQueryHandler(AppDbContext appDb)
    {
        _appDb = appDb;
    }

    public Task<CardDto[]> Handle(GetAllCardsQuery request, CancellationToken cancellationToken)
            => _appDb.TaskCards
                .Select(card => new CardDto(card.Id, card.Description, card.Responsible, card.CreatedOnUtc, card.Status))
                .ToArrayAsync(cancellationToken);
}