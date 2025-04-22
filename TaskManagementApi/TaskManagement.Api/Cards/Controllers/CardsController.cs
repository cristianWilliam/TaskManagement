using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.Common;
using TaskManagement.Api.Extensions;
using TaskManagement.Application.Cards;
using TaskManagement.Application.Cards.Create;
using TaskManagement.Application.Cards.GetAll;
using TaskManagement.Application.Cards.Move;
using TaskManagement.Core.ErrorManagement.ResultPattern;

namespace TaskManagement.Api.Cards.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardsController : BaseController
{
    private readonly ISender _sender;

    public CardsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    ///     List all cards.
    /// </summary>
    /// <param name="cancellationToken">Token for cancel action</param>
    /// <returns>List of cards</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CardDto[]))]
    public async Task<IActionResult> GetCards(CancellationToken cancellationToken = default)
    {
        return Ok(await _sender.Send(new GetAllCardsQuery(), cancellationToken));
    }

    /// <summary>
    ///     Creates a new Card
    /// </summary>
    /// <returns> The created Card </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CardDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CommonErrorResponse[]))]
    public async Task<IActionResult> CreateTodoCardAsync(CreateTodoCardRequest req,
        IValidator<CreateTodoCardRequest> validators, CancellationToken cancellationToken = default)
    {
        // Check params from request.
        var requestValidationResult = await validators.ValidateAsync(req, cancellationToken);
        if (!requestValidationResult.IsValid)
            return BadRequest(requestValidationResult.ToValidationErrorResponse());

        // Try to create command
        var commandResult = CreateTodoCardCommand.TryCreate(req.Description, req.Responsible);
        if (!commandResult.IsSuccess)
            return GetResponseError(commandResult.Errors);

        // Execute handler
        var command = commandResult.Value;
        var handlerResult = await _sender.Send(command!, cancellationToken);

        return handlerResult.Match(GetResponseError, card => Created("", card));
    }

    /// <summary>
    ///     Move the card using Status
    /// </summary>
    /// <param name="cardId"></param>
    /// <param name="req">The request</param>
    /// <param name="validator">Validator from DI</param>
    /// <param name="cancellationToken">Token for cancellation</param>
    /// <returns>The updated card in case of success</returns>
    [HttpPatch("{cardId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MoveCardDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(CommonErrorResponse[]))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(CommonErrorResponse[]))]
    public async Task<IActionResult> MoveCardAsync(Guid cardId, [FromBody] MoveCardRequest req,
        IValidator<MoveCardRequest> validator, CancellationToken cancellationToken = default)
    {
        req = req with {CardId = cardId};

        var requestValidationResult = await validator.ValidateAsync(req, cancellationToken);
        if (!requestValidationResult.IsValid)
            return BadRequest(requestValidationResult.ToValidationErrorResponse());

        var command = new MoveCardCommand(cardId, req.NewStatus);

        var handlerResult = await _sender.Send(command, cancellationToken);
        return handlerResult.Match(GetResponseError, Ok);
    }
}