using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Services;
using FitTrackApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    // GET
    [HttpGet]
    public async Task<ActionResult<PagedListResult<ExerciseListItem>>> GetAllExercises(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _exerciseService.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    // GET {id}
    [HttpGet("{exerciseId}")]
    public async Task<ActionResult<ExerciseDetailsResult>> GetExerciseById(
        [FromRoute] int exerciseId,
        CancellationToken cancellationToken = default)
    {
        var exercise = await _exerciseService.GetByIdAsync(exerciseId, cancellationToken);

        if (exercise is null)
            return NotFound(new { Message = $"Exercise with ID {exerciseId} not found." });

        return Ok(exercise);
    }
}