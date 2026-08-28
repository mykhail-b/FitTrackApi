using FitTrackApi.Application.Dto.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Feature.Workouts.Commands;
using FitTrackApi.Application.Feature.Workouts.Queries;
using MediatR;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/v1/workout")]
[ApiController]
public class WorkoutController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkoutController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


    [HttpGet("{workoutId:guid}")]
    public async Task<ActionResult<WorkoutDto>> GetWorkoutById(Guid workoutId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWorkoutByIdQuery(workoutId, CurrentUserId), ct);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedListResponse<WorkoutDto>>> GetWorkouts(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWorkoutPagedQuery(CurrentUserId), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutDto>> CreateWorkout(
        [FromBody] CreateWorkoutRequest createWorkoutRequest,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateWorkoutCommand(CurrentUserId, createWorkoutRequest), ct);
        return CreatedAtAction(nameof(GetWorkoutById), new { workoutId = result.Id }, result);
    }

    [HttpPut("{workoutId:guid}")]
    public async Task<IActionResult> UpdateWorkout(
        Guid workoutId,
        [FromBody] UpdateWorkoutRequest updateWorkoutRequest,
        CancellationToken ct)
    {
        await _mediator.Send(new UpdateWorkoutCommand(workoutId, CurrentUserId, updateWorkoutRequest), ct);
        return NoContent();
    }

    [HttpGet("activity")]
    public async Task<ActionResult<List<DateOnly>>> GetWorkoutActivity(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetWorkoutActivityQuery(CurrentUserId), ct);
        return Ok(result);
    }

    [HttpDelete("{workoutId:guid}")]
    public async Task<ActionResult> DeleteWorkout(Guid workoutId, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteWorkoutCommand(workoutId, CurrentUserId), ct);
        return result ? Ok() : NotFound();
    }
}