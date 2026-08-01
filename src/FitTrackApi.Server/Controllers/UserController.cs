using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
[Route("api/v1/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) => _userService = userService;

    [HttpGet("me")]
    public async Task<ActionResult> GetMyInfo(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var userInfo = await _userService.GetUserInfoAsync(userId, ct);
        return userInfo is null ? NotFound() : Ok(userInfo);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult> GetUserInfo(string userId, CancellationToken ct)
    {
        var userInfo = await _userService.GetUserInfoAsync(userId, ct);
        return userInfo is null ? NotFound() : Ok(userInfo);
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult> UpdateUserInfo(string userId, [FromBody] UserInfoDto dto, CancellationToken ct)
    {
        var success = await _userService.UpdateUserInfoAsync(userId, dto, ct);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult> DeleteUser(string userId, CancellationToken ct)
    {
        var success = await _userService.DeleteUserAsync(userId, ct);
        return success ? NoContent() : NotFound();
    }
}