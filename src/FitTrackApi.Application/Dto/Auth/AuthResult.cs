namespace FitTrackApi.Application.Dto.Auth;

/// <summary>
/// Represents the result of an authentication operation, indicating whether it succeeded
/// and providing an optional error message when it fails.
/// </summary>
public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }

    public static AuthResult Success() => new() { Succeeded = true };
    public static AuthResult Fail(string error) => new() { Succeeded = false, Error = error };
}
