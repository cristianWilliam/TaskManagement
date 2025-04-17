using MediatR;
using TaskManagement.Core.ErrorManagement;

namespace TaskManagement.Application.Cards.GetAll;

public record GetAllCardsQuery(): IRequest<CardDto[]>;