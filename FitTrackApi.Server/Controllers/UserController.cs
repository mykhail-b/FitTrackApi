using System.Security.Claims;
using FitTrackApi.Core.Dto.User;
using FitTrackApi.Server.Cqrs.Handlers.UserHandlers;
using FitTrackApi.Server.Cqrs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{

    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public UserController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    //GET me - Get info of the currently authenticated user (based on the auth cookie, not a URL id)
    [HttpGet("me")]
    public async Task<ActionResult> GetMyInfo(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return Unauthorized();

        var query = new GetUserInfoQuery(userId);
        var userInfo = await _queryDispatcher.Dispatch<GetUserInfoQuery, UserInfoDto>(query, cancellationToken);

        if (userInfo is null)
            return NotFound();

        return Ok(userInfo);
    }

    //GET UserId - Get basic user account info like Fullname and email
    [HttpGet("{userId}")]
    public async Task<ActionResult> GetUserInfo(string userId, CancellationToken cancellationToken)
    {
        var query = new GetUserInfoQuery(userId);
        var userInfo = await _queryDispatcher.Dispatch<GetUserInfoQuery, UserInfoDto>(query, cancellationToken);

        if (userInfo is null)
            return NotFound();

        return Ok(userInfo);
    }

    //POST ContactInfo UserId - Update user Email or phone for example
    [HttpPost("{userId}")]
    public async Task<ActionResult> UpdateUserInfo(string userId, [FromBody] UserInfoDto updatedUserData, CancellationToken cancellationToken)
    {
        var command = new UpdateUserInfoCommand(userId, updatedUserData);
        var userInfo = await _commandDispatcher.Dispatch<UpdateUserInfoCommand, bool>(command, cancellationToken);

        if (!userInfo)
            return NotFound();

        return NoContent();
    }

    //DELETE UserId - delete User account
    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(string userId, CancellationToken cancellationToken)
    {
        var command = new RemoveUserCommand(userId);
        var removed = await _commandDispatcher.Dispatch<RemoveUserCommand, bool>(command, cancellationToken);

        if (!removed)
            return NotFound();

        return NoContent();
    }

    //---------------

    //GET UserId - Get users body metric by personal user id
    [HttpGet("{userId}/body-metric")]
    public async Task<ActionResult<BodyMetricDto>> GetBodyMetric(string userId, CancellationToken cancellationToken)
    {
        var query = new GetBodyMetricQuery(userId);
        var metric = await _queryDispatcher.Dispatch<GetBodyMetricQuery, BodyMetricDto>(query, cancellationToken);
        return Ok(metric);
    }


    [HttpPut("{userId}/body-metric")]
    public async Task<ActionResult<BodyMetricDto>> UpdateBodyMetric(string userId, [FromBody] BodyMetricDto bodyMetric, CancellationToken cancellationToken)
    {
        var command = new UpdateBodyMetricCommand(userId, bodyMetric);
        var result = await _commandDispatcher.Dispatch<UpdateBodyMetricCommand, BodyMetricDto>(command, cancellationToken);
        return Ok(result);
    }
}