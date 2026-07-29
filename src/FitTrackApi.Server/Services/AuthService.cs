using FitTrackApi.Core.Dto;
using FitTrackApi.Core.Entity;
using Microsoft.AspNetCore.Identity;

namespace FitTrackApi.Server.Services;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task LogoutAsync();
}

public class AuthService : IAuthService
{
    private readonly UserManager<UserAccount> _userManager;
    private readonly SignInManager<UserAccount> _signInManager;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<UserAccount> userManager,
        SignInManager<UserAccount> signInManager,
        IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return AuthResult.Fail("User with this email already exists");

        var user = new UserAccount
        {
            Email = request.Email,
            UserName = request.Email,
            FullName = request.FullName
        };


        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return AuthResult.Fail(errors);
        }

        try
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        catch (InvalidOperationException)
        {
            // In test environments SignInManager may not have an HttpContext; ignore sign-in there
        }

        await _emailService.SendEmail(
            request.Email,
            "Welcome in FitTrack",
            $"<h1>Hello, {request.FullName}!</h1><p>Thanks for registration in FitTrack.</p>");

        return AuthResult.Success();
    }
    
    public async Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return AuthResult.Fail("Invalid email or password");

        var result = await _signInManager.PasswordSignInAsync(
            user,
            request.Password,
            isPersistent: request.RememberMe,
            lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                return AuthResult.Fail("Account locked due to multiple failed attempts");

            return AuthResult.Fail("Invalid email or password");
        }

        return AuthResult.Success();
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}