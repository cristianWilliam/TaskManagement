using MediatR;

namespace TaskManagement.Application.Cards.GetAll;

public record GetAllCardsQuery : IRequest<CardDto[]>;