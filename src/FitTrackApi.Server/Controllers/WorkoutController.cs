using FitTrackApi.Application.Dto.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FitTrackApi.Application.Dto;
using FitTrackApi.Server.Services.Public;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/v1/workout")]
[ApiController]
public class WorkoutController : ControllerBase
{
    private readonly IWorkoutService _service;

    public WorkoutController(IWorkoutService service)
    {
        _service = service;
    }

    private Guid CurrentAccountId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("{workoutId:guid}")]
    public async Task<ActionResult<WorkoutDto>> GetWorkoutById(Guid workoutId, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(workoutId, CurrentAccountId, ct);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedListResponse<WorkoutDto>>> GetWorkouts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var result = await _service.GetPagedAsync(CurrentAccountId, pageNumber, pageSize, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutDto>> CreateWorkout(
        [FromBody] CreateWorkoutRequest createWorkoutRequest,
        CancellationToken ct)
    {
        var result = await _service.CreateAsync(CurrentAccountId, createWorkoutRequest, ct);
        return CreatedAtAction(nameof(GetWorkoutById), new { workoutId = result.Id }, result);
    }

    [HttpPut("{workoutId:guid}")]
    public async Task<IActionResult> UpdateWorkout(
        Guid workoutId,
        [FromBody] UpdateWorkoutRequest updateWorkoutRequest,
        CancellationToken ct)
    {
        await _service.UpdateAsync(workoutId, CurrentAccountId, updateWorkoutRequest, ct);
        return NoContent();
    }

    [HttpGet("activity")]
    public async Task<ActionResult<List<DateOnly>>> GetWorkoutActivity(CancellationToken ct)
    {
        var result = await _service.GetActivityAsync(CurrentAccountId, ct);
        return Ok(result);
    }

    [HttpDelete("{workoutId:guid}")]
    public async Task<ActionResult> DeleteWorkout(Guid workoutId, CancellationToken ct)
    {
        var result = await _service.DeleteAsync(workoutId, CurrentAccountId, ct);
        return result ? Ok() : NotFound();
    }
}