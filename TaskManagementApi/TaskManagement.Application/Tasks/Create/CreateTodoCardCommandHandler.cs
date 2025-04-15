using MediatR;

using TaskManagement.Application.Providers;
using TaskManagement.Core.ErrorManagement;
using TaskManagement.Domain;
using TaskManagement.Domain.ValueObjects;
using TaskManagement.Infra;

namespace TaskManagement.Application.Tasks.Create;

public record CreateTodoCardCommand(
    CardDescription Description,
    CardResponsible Responsible) : IRequest<Result<CreateTodoCardDto>>;

internal sealed class CreateTodoCardCommandHandler : IRequestHandler<
    CreateTodoCardCommand, Result<CreateTodoCardDto>>
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

    public async Task<Result<CreateTodoCardDto>> Handle(
        CreateTodoCardCommand request, CancellationToken cancellationToken)
    {
        var newCardId = _guidProvider.GenerateSequential();

        var newCard = Card.CreateToDoCard(newCardId, request.Description,
            request.Responsible, _dateTimeProvider);

        await _context.TaskCards.AddAsync(newCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateTodoCardDto(newCard.Id, newCard.Description.ToString(),
            newCard.Responsible.ToString(), newCard.CreatedOnUtc,
            newCard.Status);
    }
}