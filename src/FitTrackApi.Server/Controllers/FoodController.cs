using FitTrackApi.Application.Dto.Food;
using FitTrackApi.Application.Feature.Foods.Commands;
using FitTrackApi.Application.Feature.Foods.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Route("api/v1/food")]
[ApiController]
public class FoodController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoodController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetFoodPagedQuery(pageNumber, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{foodId:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetFoodByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFoodRequest createFoodRequest, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new CreateFoodCommand(createFoodRequest), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{foodId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute]Guid id,
        [FromBody] UpdateFoodRequest updateFoodRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new UpdateFoodCommand(id, updateFoodRequest), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{foodId:guid}")]
    public async Task<IActionResult> Delete(
        [FromBody]Guid id,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteFoodCommand(id), cancellationToken);
        return NoContent();
    }
}