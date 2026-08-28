using FitTrackApi.Application.Dto.ApiResponses;
using FitTrackApi.Application.Dto.Auth;
using FitTrackApi.Infrastructure.IdentityEntity;
using FitTrackApi.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitTrackApi.Server.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<UserAccount> _userManager;

    public AuthController(
        IAuthService authService,
        UserManager<UserAccount> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(
            request,
            cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse(result.Error));
        }

        return StatusCode(
            StatusCodes.Status201Created,
            new { message = "Registration successful" });
    }

    [HttpPost("login")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(
            request,
            cancellationToken);

        if (!result.Succeeded)
        {
            return Unauthorized(new ApiErrorResponse(result.Error));
        }

        return Ok(new { message = "Login Successful" });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();

        return Ok(new { message = "Logged out" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Unauthorized();
        }

        var response = new UserResponse(
            user.Id,
            user.UserName,
            user.FullName
        );

        return Ok(new ApiSuccessResponse(
            "User info",
            response
        ));
    }
}