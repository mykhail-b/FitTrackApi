namespace FitTrackApi.Core.Dto;

public class RegisterRequest
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; } = false;
}

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }

    public static AuthResult Success() => new() { Succeeded = true };
    public static AuthResult Fail(string error) => new() { Succeeded = false, Error = error };
}