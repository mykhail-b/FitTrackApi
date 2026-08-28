using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Feature.Exercises.Commands;
using FitTrackApi.Application.Feature.Exercises.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExerciseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<ExerciseShortResponse>>> GetAllExercises(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetExercisePagedQuery(pageNumber, pageSize),  cancellationToken);
        
        return Ok(result);
    }

    // GET {id}
    [HttpGet("{exerciseId:guid}")]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseById(
        [FromRoute] Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetExerciseByIdQuery(exerciseId), cancellationToken);

        return Ok(result);
    }
    
    // POST
    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> CreateExercise(
        [FromBody] CreateExerciseRequest createExerciseRequest, 
        CancellationToken cancellationToken = default)
    {
        var  result = await _mediator.Send(new CreateExerciseCommand(createExerciseRequest), cancellationToken);
        return CreatedAtAction(nameof(GetExerciseById), new { exerciseId = result.Id }, result);
    }
    //PUT {id}
    [HttpPut("{exerciseId:guid}")]
    public async Task<ActionResult<ExerciseResponse>> UpdateExercise(
        [FromRoute] Guid exerciseId,
        [FromBody] UpdateExerciseRequest updateExerciseRequest, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new UpdateExerciseCommand(exerciseId, updateExerciseRequest), cancellationToken);
        return Ok(result);
    }
    //DELETE {id}
    [HttpDelete("{exerciseId:guid}")]
    public async Task<ActionResult<ExerciseResponse>> DeleteExercise([FromRoute] Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var result = _mediator.Send(new DeleteExerciseCommand(exerciseId), cancellationToken);
        return NoContent();
    }
}