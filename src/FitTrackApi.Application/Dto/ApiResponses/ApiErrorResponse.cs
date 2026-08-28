namespace FitTrackApi.Application.Dto.ApiResponses;

/// <summary>
/// Represents a standardized error response returned by the API.
/// </summary>
/// <param name="Error">
/// A human‑readable description of the error that occurred.
/// </param>
public record ApiErrorResponse(string Error);
