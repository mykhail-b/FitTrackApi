using FitTrackApi.Application.Dto.User;
using FitTrackApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/v1/body-metrics")]
[ApiController]
public class BodyMetricsController : ControllerBase
{
    private readonly IBodyMetricService _bodyMetricService;

    public BodyMetricsController(IBodyMetricService bodyMetricService)
    {
        _bodyMetricService = bodyMetricService;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<BodyMetricDto>> GetBodyMetric(string userId, CancellationToken cancellationToken)
    {
        var metric = await _bodyMetricService.GetAsync(userId, cancellationToken);
        return Ok(metric);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<BodyMetricDto>> UpdateBodyMetric(string userId, [FromBody] BodyMetricDto bodyMetric, CancellationToken cancellationToken)
    {
        var result = await _bodyMetricService.UpdateAsync(userId, bodyMetric, cancellationToken);
        return Ok(result);
    }
}