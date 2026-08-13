namespace FitTrackApi.Application.Dto.Auth;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }

    public static AuthResult Success() => new() { Succeeded = true };
    public static AuthResult Fail(string error) => new() { Succeeded = false, Error = error };
}
