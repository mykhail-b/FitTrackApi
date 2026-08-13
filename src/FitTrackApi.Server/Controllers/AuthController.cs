using FitTrackApi.Application.Dto.Auth;
using FitTrackApi.Application.Dto.Responses;
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

    public AuthController(IAuthService authService, UserManager<UserAccount> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new ApiSuccessResponse
        {
            Message = "Registration successful",
            Data = new { email = request.Email, fullName = request.FullName }
        });
    }

    [HttpPost("login")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);

        if (!result.Succeeded)
            return Unauthorized(new { error = result.Error });

        return Ok(new ApiSuccessResponse
        {
            Message = "Login successful"
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();

        return Ok(new ApiSuccessResponse
        {
            Message = "Logged out"
        });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
            return Unauthorized();

        return Ok(new ApiSuccessResponse
        {
            Message = "User info",
            Data = new
            {
                id = user.Id,
                username = user.UserName,
                fullName = user.FullName
            }
        });
    }
}