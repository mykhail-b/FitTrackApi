using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET api/workout
    [HttpGet]
    public async Task<ActionResult<List<WorkoutDto>>> GetAllUserWorkouts(CancellationToken cancellationToken)
    {
        var result = await _workoutService.GetAllForUserAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    // GET api/workout/{workoutId}
    [HttpGet("{workoutId:guid}")]
    public async Task<ActionResult<WorkoutDto>> GetUserWorkoutById(Guid workoutId, CancellationToken cancellationToken)
    {
        var result = await _workoutService.GetByIdAsync(workoutId, cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    // POST api/workout
    [HttpPost]
    public async Task<ActionResult> CreateWorkout([FromBody] WorkoutDto workout, CancellationToken cancellationToken)
    {
        var result = await _workoutService.CreateAsync(CurrentUserId, workout.Date, workout.Notes, workout.Exercises, cancellationToken);
        return result ? Ok() : BadRequest();
    }

    // PUT api/workout/{workoutId}
    [HttpPut("{workoutId:guid}")]
    public async Task<ActionResult> UpdateWorkout(Guid workoutId, [FromBody] WorkoutDto updatedWorkout, CancellationToken cancellationToken)
    {
        var result = await _workoutService.UpdateAsync(workoutId, updatedWorkout.Date, updatedWorkout.Notes, updatedWorkout.Exercises, cancellationToken);
        return result ? Ok() : NotFound();
    }

    // DELETE api/workout/{workoutId}
    [HttpDelete("{workoutId:guid}")]
    public async Task<ActionResult> DeleteWorkout(Guid workoutId, CancellationToken cancellationToken)
    {
        var result = await _workoutService.RemoveAsync(workoutId, cancellationToken);
        return result ? Ok() : NotFound();
    }
}