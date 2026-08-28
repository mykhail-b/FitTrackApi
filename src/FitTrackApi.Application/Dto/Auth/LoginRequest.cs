namespace FitTrackApi.Application.Dto.Auth;

/// <summary>
/// Represents the model for the user login form.
/// </summary>
public record LoginRequest(string Email, string Password, bool RememberMe);