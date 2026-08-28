namespace FitTrackApi.Application.Dto.Auth;

/// <summary>
/// Represents the model for the user registration form.
/// </summary>
public record RegisterRequest(string FullName, string Email, string Password);