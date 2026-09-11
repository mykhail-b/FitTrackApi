using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Server.Services.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _service;

    public ExerciseController(IExerciseService service)
    {
        _service = service;
    }

    // GET
    [HttpGet]
    public async Task<ActionResult<PagedListResponse<ExerciseShortResponse>>> GetAllExercises(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetPagedAsync(pageNumber, pageSize, cancellationToken);

        return Ok(result);
    }

    // GET {id}
    [HttpGet("{exerciseId:guid}")]
    public async Task<ActionResult<ExerciseResponse>> GetExerciseById(
        [FromRoute] Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.GetByIdAsync(exerciseId, cancellationToken);

        return Ok(result);
    }
    
    // POST
    [HttpPost]
    public async Task<ActionResult<ExerciseResponse>> CreateExercise(
        [FromBody] CreateExerciseRequest createExerciseRequest, 
        CancellationToken cancellationToken = default)
    {
        var result = await _service.CreateAsync(createExerciseRequest, cancellationToken);

        return Ok(result);
    }
    //PUT {id}
    [HttpPut("{exerciseId:guid}")]
    public async Task<ActionResult<ExerciseResponse>> UpdateExercise(
        [FromRoute] Guid exerciseId,
        [FromBody] UpdateExerciseRequest updateExerciseRequest, CancellationToken cancellationToken = default)
    {
        var result = await _service.UpdateAsync(exerciseId, updateExerciseRequest, cancellationToken);

        return Ok(result);
    }
    //DELETE {id}
    [HttpDelete("{exerciseId:guid}")]
    public async Task<ActionResult<ExerciseResponse>> DeleteExercise([FromRoute] Guid exerciseId,
        CancellationToken cancellationToken = default)
    {
        var result = await _service.DeleteAsync(exerciseId, cancellationToken);

        return Ok(result);
    }
}