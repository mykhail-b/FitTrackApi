using FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;
using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Core.Dto.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WorkoutController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public WorkoutController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET api/workout
    [HttpGet]
    public async Task<ActionResult<List<WorkoutDto>>> GetAllUserWorkouts(CancellationToken cancellationToken)
    {
        var query = new GetAllUserWorkoutsQuery(CurrentUserId);
        var result = await _queryDispatcher.Dispatch<GetAllUserWorkoutsQuery, List<WorkoutDto>>(query, cancellationToken);
        return Ok(result);
    }

    // GET api/workout/{workoutId}
    [HttpGet("{workoutId:guid}")]
    public async Task<ActionResult<WorkoutDto>> GetUserWorkoutById(Guid workoutId, CancellationToken cancellationToken)
    {
        var query = new GetWorkoutByIdQuery(workoutId);
        var result = await _queryDispatcher.Dispatch<GetWorkoutByIdQuery, WorkoutDto?>(query, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // POST api/workout
    [HttpPost]
    public async Task<ActionResult> CreateWorkout([FromBody] WorkoutDto workout, CancellationToken cancellationToken)
    {
        var command = new CreateWorkoutCommand(CurrentUserId, workout.Date, workout.Notes, workout.Exercises);
        var result = await _commandDispatcher.Dispatch<CreateWorkoutCommand, bool>(command, cancellationToken);

        return result ? Ok() : BadRequest();
    }

    // PUT api/workout/{workoutId}
    [HttpPut("{workoutId:guid}")]
    public async Task<ActionResult> UpdateWorkout(Guid workoutId, [FromBody] WorkoutDto updatedWorkout, CancellationToken cancellationToken)
    {
        var command = new UpdateWorkoutCommand(workoutId, updatedWorkout.Date, updatedWorkout.Notes, updatedWorkout.Exercises);
        var result = await _commandDispatcher.Dispatch<UpdateWorkoutCommand, bool>(command, cancellationToken);

        return result ? Ok() : NotFound();
    }

    // DELETE api/workout/{workoutId}
    [HttpDelete("{workoutId:guid}")]
    public async Task<ActionResult> DeleteWorkout(Guid workoutId, CancellationToken cancellationToken)
    {
        var command = new RemoveWorkoutCommand(workoutId);
        var result = await _commandDispatcher.Dispatch<RemoveWorkoutCommand, bool>(command, cancellationToken);

        return result ? Ok() : NotFound();
    }
}