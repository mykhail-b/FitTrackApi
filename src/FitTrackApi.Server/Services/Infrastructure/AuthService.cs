using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
namespace FitTrackApi.Server.Services.Infrastructure;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<(bool Success, string? Error)> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task LogoutAsync();
}


public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return (false, "User with this email already exists");

        var user = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return (false, errors);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);

        await _emailService.SendEmail(
            request.Email,
            "Welcome in FitTrack",
            $"<h1>Hello!</h1><p>Thanks for registration in FitTrack.</p>");

        return (true, null);
    }


    public async Task<(bool Success, string? Error)> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return (false, "Invalid email or password");

        var result = await _signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return (false, "Account locked due to multiple failed attempts");

            return (false, "Invalid email or password");
        }

        return (true, null);
    }


    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}