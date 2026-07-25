using FitTrackApi.Core.Dto.Exercise;
using FitTrackApi.Server.Cqrs.Handlers.ExerciseHandlers;
using FitTrackApi.Server.Cqrs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;

    public ExerciseController(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    //GET
    [HttpGet]
    public async Task<ActionResult<PagedListResult<ExerciseListItemResult>>> GetAllExercises(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExercisesPagedQuery(pageNumber, pageSize);

        var result = await _queryDispatcher.Dispatch<GetExercisesPagedQuery, PagedListResult<ExerciseListItemResult>>(query, cancellationToken);

        return Ok(result);
    }

    //GET{ID}
    [HttpGet("{exerciseId}")]
    public async Task<ActionResult<ExerciseDetailsResult>> GetExerciseById(
        [FromRoute] int exerciseId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExerciseByIdQuery(exerciseId);
        var exercise = await _queryDispatcher.Dispatch<GetExerciseByIdQuery, ExerciseDetailsResult?>(query, cancellationToken);

        if (exercise == null)
        {
            return NotFound(new { Message = $"Exercise with ID {exerciseId} not founded." });
        }

        return Ok(exercise);
    }
}
